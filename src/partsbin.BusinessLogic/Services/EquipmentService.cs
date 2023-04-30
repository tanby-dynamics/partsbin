using partsbin.BusinessLogic.Helpers;
using partsbin.BusinessLogic.Models;
using File = partsbin.BusinessLogic.Models.File;

namespace partsbin.BusinessLogic.Services;

public interface IEquipmentService
{
    Task<Equipment> AddEquipment(Equipment equipment);
    Task UpdateEquipment(Equipment equipment);
    Task<Equipment> GetEquipment(int id);
    Task<IEnumerable<Equipment>> GetAllEquipment(string? byType = null, string? qualifier = null);
    Task<IEnumerable<Equipment>> GetDeletedEquipment();
    Task EmptyRubbishBin();
    Task<Equipment> Duplicate(Equipment source);
    Task<IEnumerable<Equipment>> GetEquipmentWithIds(IEnumerable<int> ids);
    Task<int> UpdateLocations(string originalLocation, string newLocation);
}

public class EquipmentService : IEquipmentService
{
    private readonly IDbFactory _dbFactory;
    private readonly IEquipmentSearchService _equipmentSearchService;
    private readonly IImageService _imageService;
    private readonly IFileService _fileService;

    public EquipmentService(
        IDbFactory dbFactory, 
        IEquipmentSearchService equipmentSearchService, 
        IImageService imageService,
        IFileService fileService)
    {
        _dbFactory = dbFactory;
        _equipmentSearchService = equipmentSearchService;
        _imageService = imageService;
        _fileService = fileService;
    }
    
    public async Task<Equipment> AddEquipment(Equipment equipment)
    {
        using var db = await _dbFactory.GetDatabase();

        await db.GetCollection<Equipment>().InsertAsync(equipment);
        _equipmentSearchService.IndexEquipment(equipment);

        return equipment;
    }

    public async Task UpdateEquipment(Equipment equipment)
    {
        _equipmentSearchService.RemoveEquipment(equipment);
        _equipmentSearchService.IndexEquipment(equipment);

        using var db = await _dbFactory.GetDatabase();
        await db.GetCollection<Equipment>().UpdateAsync(equipment);
    }

    public async Task<Equipment> GetEquipment(int id)
    {
        using var db = await _dbFactory.GetDatabase();

        return await db.GetCollection<Equipment>().FindByIdAsync(id);
    }

    public async Task<IEnumerable<Equipment>> GetAllEquipment(string? byType = null, string? qualifier = null)
    {
        using var db = await _dbFactory.GetDatabase();

        if (byType is not null && string.IsNullOrEmpty(qualifier))
        {
            return Array.Empty<Equipment>();
        }
        
        var equipment = await db.GetCollection<Equipment>().Query()
            .Where(x => !x.IsDeleted)
            .ToListAsync();

        // Apply the optional 'byType/qualifier' filter
        var filteredParts = (byType ?? string.Empty) switch
        {
            "by-equipment-type" => equipment.Where(x => x.EquipmentType != null && x.EquipmentType.ToLower() == qualifier),
            "by-equipment-name" => equipment.Where(x => x.EquipmentName != null && x.EquipmentName.ToLower() == qualifier),
            "by-manufacturer" => equipment.Where(x => x.Manufacturer != null && x.Manufacturer.ToLower() == qualifier),
            "by-location" => equipment.Where(x => x.Location != null && x.Location.ToLower() == qualifier),
            _ => equipment
        };
        
        return filteredParts.ToList();
    }

    public async Task<IEnumerable<Equipment>> GetDeletedEquipment()
    {
        using var db = await _dbFactory.GetDatabase();

        return await db.GetCollection<Equipment>().Query()
            .Where(x => x.IsDeleted)
            .ToListAsync();
    }

    public async Task EmptyRubbishBin()
    {
        using var db = await _dbFactory.GetDatabase();

        var deletedItems = await db.GetCollection<Equipment>().Query()
            .Where(x => x.IsDeleted)
            .ToArrayAsync();
        
        foreach (var item in deletedItems)
        {
            // Drop from search index
            _equipmentSearchService.RemoveEquipment(item);

            // Delete Images and Files
            await db.GetCollection<Image>().DeleteManyAsync(x => x.EquipmentId == item.Id);
            await db.GetCollection<File>().DeleteManyAsync(x => x.EquipmentId == item.Id);
            
            // Delete the item (Suppliers and Documentation are part of the item)
            await db.GetCollection<Equipment>().DeleteAsync(item.Id);
        }
    }

    public async Task<Equipment> Duplicate(Equipment source)
    {
        var duplicate = await AddEquipment(source.DeepClone());

        var sourceImages = await _imageService.GetAllImages(source);
        foreach (var image in sourceImages)
        {
            await _imageService.DuplicateImage(image, duplicate);
        }

        var sourceFiles = await _fileService.GetAllFiles(source);
        foreach (var file in sourceFiles)
        {
            await _fileService.DuplicateFile(file, duplicate);
        }

        return duplicate;
    }

    public async Task<IEnumerable<Equipment>> GetEquipmentWithIds(IEnumerable<int> ids)
    {
        using var db = await _dbFactory.GetDatabase();
        var collection = db.GetCollection<Equipment>();
        var items = await collection.Query()
            .Where(x => ids.Contains(x.Id))
            .ToListAsync();

        return items;
    }

    public async Task<int> UpdateLocations(string originalLocation, string newLocation)
    {
        using var db = await _dbFactory.GetDatabase();
        
        var items = await db.GetCollection<Equipment>().Query()
            .Where(x => x.Location == originalLocation)
            .ToArrayAsync();
        
        foreach (var item in items) 
        {
            item.Location = newLocation;
            await UpdateEquipment(item);
        }

        return items.Count();
    }
}