using partsbin.BusinessLogic.Helpers;
using partsbin.BusinessLogic.Models;
using File = partsbin.BusinessLogic.Models.File;

namespace partsbin.BusinessLogic.Services;

public interface IPartService
{
    Task<Part> AddPart(Part part);
    Task UpdatePart(Part part);
    Task<Part> GetPart(int id);
    Task<IEnumerable<Part>> GetAllParts(string? byType = null, string? qualifier = null);
    Task<IEnumerable<Part>> GetDeletedParts();
    Task EmptyRubbishBin();
    Task<Part> Duplicate(Part source);
    Task<IEnumerable<Part>> GetPartsWithIds(IEnumerable<int> ids);
    Task<bool> IsThisDuplicatePartNumber(string partNumber, int? excludePartId = null);
}

public class PartService : IPartService
{
    private readonly IDbFactory _dbFactory;
    private readonly IPartSearchService _partSearchService;
    private readonly IImageService _imageService;
    private readonly IFileService _fileService;

    public PartService(
        IDbFactory dbFactory, 
        IPartSearchService partSearchService, 
        IImageService imageService,
        IFileService fileService)
    {
        _dbFactory = dbFactory;
        _partSearchService = partSearchService;
        _imageService = imageService;
        _fileService = fileService;
    }
    
    public async Task<Part> AddPart(Part part)
    {
        using var db = await _dbFactory.GetDatabase();

        await db.GetCollection<Part>().InsertAsync(part);
        _partSearchService.IndexPart(part);

        return part;
    }

    public async Task UpdatePart(Part part)
    {
        _partSearchService.RemovePart(part);
        _partSearchService.IndexPart(part);

        using var db = await _dbFactory.GetDatabase();
        await db.GetCollection<Part>().UpdateAsync(part);
    }

    public async Task<Part> GetPart(int id)
    {
        using var db = await _dbFactory.GetDatabase();

        return await db.GetCollection<Part>().FindByIdAsync(id);
    }

    public async Task<IEnumerable<Part>> GetAllParts(string? byType = null, string? qualifier = null)
    {
        using var db = await _dbFactory.GetDatabase();

        if (byType is not null && string.IsNullOrEmpty(qualifier))
        {
            return Array.Empty<Part>();
        }
        
        var parts = await db.GetCollection<Part>().Query()
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        // Apply the optional 'byType/qualifier' filter
        var filteredParts = (byType ?? string.Empty) switch
        {
            "by-part-type" => parts.Where(x => x.PartType != null && x.PartType.ToLower() == qualifier),
            "by-range" => parts.Where(x => x.Range != null && x.Range.ToLower() == qualifier),
            "by-part-name" => parts.Where(x => x.PartName != null && x.PartName.ToLower() == qualifier),
            "by-manufacturer" => parts.Where(x => x.Manufacturer != null && x.Manufacturer.ToLower() == qualifier),
            "by-location" => parts.Where(x => x.Location != null && x.Location.ToLower() == qualifier),
            "by-part-number" => parts.Where(x => x.PartNumber != null && x.PartNumber.ToLower() == qualifier),
            _ => parts
        };
        
        return filteredParts.ToList();
    }

    public async Task<IEnumerable<Part>> GetDeletedParts()
    {
        using var db = await _dbFactory.GetDatabase();

        return await db.GetCollection<Part>().Query()
            .Where(x => x.IsDeleted)
            .ToListAsync();
    }

    public async Task EmptyRubbishBin()
    {
        using var db = await _dbFactory.GetDatabase();

        var deletedParts = await db.GetCollection<Part>().Query()
            .Where(x => x.IsDeleted)
            .ToArrayAsync();
        
        foreach (var part in deletedParts)
        {
            // Drop from search index
            _partSearchService.RemovePart(part);

            // Delete Images and Files
            await db.GetCollection<Image>().DeleteManyAsync(x => x.PartId == part.Id);
            await db.GetCollection<File>().DeleteManyAsync(x => x.PartId == part.Id);
            
            // Delete the part (Suppliers and Documentation are part of the Part)
            await db.GetCollection<Part>().DeleteAsync(part.Id);
        }
    }

    public async Task<Part> Duplicate(Part source)
    {
        var duplicatePart = await AddPart(source.DeepClone());

        var sourceImages = await _imageService.GetAllImagesForPart(source);
        foreach (var image in sourceImages)
        {
            await _imageService.DuplicateImageIntoPart(image, duplicatePart);
        }

        var sourceFiles = await _fileService.GetAllFilesForPart(source);
        foreach (var file in sourceFiles)
        {
            await _fileService.DuplicateFileIntoPart(file, duplicatePart);
        }

        return duplicatePart;
    }

    public async Task<IEnumerable<Part>> GetPartsWithIds(IEnumerable<int> ids)
    {
        using var db = await _dbFactory.GetDatabase();
        var partsCollection = db.GetCollection<Part>();
        var parts = await partsCollection.Query()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        return parts;
    }

    /// <summary>
    /// Note that this does an exact search, different capitalization
    /// etc will return a false result
    /// </summary>
    /// <param name="partNumber"></param>
    /// <param name="excludePartId">If provided, excludes the identified part (for updates)</param>
    /// <returns>True if the provided part number is already used in one or more existing parts</returns>
    public async Task<bool> IsThisDuplicatePartNumber(string partNumber, int? excludePartId = null)
    {
        using var db = await _dbFactory.GetDatabase();
        var partsCollection = db.GetCollection<Part>();
        var matchingPart = await partsCollection.Query()
            .Where (x => excludePartId == null || x.Id != excludePartId)
            .Where(x => x.PartNumber == partNumber)
            .FirstOrDefaultAsync();

        return matchingPart is not null;
    }
}