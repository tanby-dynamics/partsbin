using System.Collections;
using System.Xml.Linq;
using LiteDB;
using partsbin.Models;

namespace partsbin.Services;

public interface IPartService
{
    Part AddPart(Part part);
    void UpdatePart(Part part);
    Part GetPart(int id);
    IEnumerable<Part> GetAllParts(string? byType = null, string? qualifier = null);
    IEnumerable<Part> GetDeletedParts();
    void EmptyRubbishBin();
    Part Duplicate(Part source);
    IEnumerable<Part> GetPartsWithIds(IEnumerable<int> ids);
    bool CheckForDuplicatePartNumber(string partNumber, int? excludePartId = null);
}

public class PartService : IPartService
{
    private readonly IDbFactory _dbFactory;
    private readonly IPartSearchService _partSearchService;

    public PartService(IDbFactory dbFactory, IPartSearchService partSearchService)
    {
        _dbFactory = dbFactory;
        _partSearchService = partSearchService;
    }
    
    public Part AddPart(Part part)
    {
        using var db = _dbFactory.GetDatabase();

        db.GetCollection<Part>().Insert(part);
        _partSearchService.IndexPart(part);

        return part;
    }

    public void UpdatePart(Part part)
    {
        using var db = _dbFactory.GetDatabase();
        _partSearchService.RemovePart(part);
        _partSearchService.IndexPart(part);

        db.GetCollection<Part>().Update(part);
    }

    public Part GetPart(int id)
    {
        using var db = _dbFactory.GetDatabase();

        return db.GetCollection<Part>().FindById(id);
    }

    public IEnumerable<Part> GetAllParts(string? byType = null, string? qualifier = null)
    {
        using var db = _dbFactory.GetDatabase();

        if (byType is not null && string.IsNullOrEmpty(qualifier))
        {
            return Array.Empty<Part>();
        }
        
        var parts = db.GetCollection<Part>()
            .FindAll()
            .Where(x => !x.IsDeleted);

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

    public IEnumerable<Part> GetDeletedParts()
    {
        using var db = _dbFactory.GetDatabase();

        return db.GetCollection<Part>()
            .Find(x => x.IsDeleted)
            .ToList();
    }

    public void EmptyRubbishBin()
    {
        using var db = _dbFactory.GetDatabase();

        // Drop from search index
        var deletedParts = db.GetCollection<Part>().Query()
            .Where(x => x.IsDeleted)
            .ToArray();
        foreach (var part in deletedParts) _partSearchService.RemovePart(part);
        
        // Delete from database
        db.GetCollection<Part>()
            .DeleteMany(x => x.IsDeleted);
    }

    public Part Duplicate(Part source) => AddPart(source.DeepClone());

    public IEnumerable<Part> GetPartsWithIds(IEnumerable<int> ids)
    {
        using var db = _dbFactory.GetDatabase();
        var partsCollection = db.GetCollection<Part>();
        var parts = partsCollection.Query()
            .Where(x => ids.Contains(x.Id))
            .ToList();

        return parts;
    }

    /// <summary>
    /// Note that this does an exact search, different capitalization
    /// etc will return a false result
    /// </summary>
    /// <param name="partNumber"></param>
    /// <param name="excludePartId">If provided, excludes the identified part (for updates)</param>
    /// <returns>True if the provided part number is already used in one or more existing parts</returns>
    public bool CheckForDuplicatePartNumber(string partNumber, int? excludePartId = null)
    {
        using var db = _dbFactory.GetDatabase();
        var partsCollection = db.GetCollection<Part>();
        var matchingPart = partsCollection.Query()
            .Where (x => excludePartId == null || x.Id != excludePartId)
            .Where(x => x.PartNumber == partNumber)
            .FirstOrDefault();

        return matchingPart is not null;
    }
}