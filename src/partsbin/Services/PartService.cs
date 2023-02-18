using LiteDB;
using partsbin.Models;

namespace partsbin.Services;

public interface IPartService
{
    Part AddPart(Part part);
    void UpdatePart(Part part);
    Part GetPart(int id);
    IEnumerable<Part> GetAllParts(string? byType = null, string? qualifier = null);
}

public class PartService : IPartService
{
    private readonly IDbFactory _dbFactory;

    public PartService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }
    
    public Part AddPart(Part part)
    {
        using var db = _dbFactory.GetDatabase();

        db.GetCollection<Part>().Insert(part);
        CachePartFields(db, part);

        return part;
    }

    public void UpdatePart(Part part)
    {
        using var db = _dbFactory.GetDatabase();

        db.GetCollection<Part>().Update(part);
        CachePartFields(db, part);
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
        
        var parts = db.GetCollection<Part>().FindAll();

        // Apply the 'byType/qualifier' filter
        var filteredParts = byType switch
        {
            "by-part-type" => parts.Where(x => x.PartType != null && x.PartType.ToLower() == qualifier),
            "by-range" => parts.Where(x => x.Range != null && x.Range.ToLower() == qualifier),
            "by-part-name" => parts.Where(x => x.PartName != null && x.PartName.ToLower() == qualifier),
            "by-manufacturer" => parts.Where(x => x.Manufacturer != null && x.Manufacturer.ToLower() == qualifier),
            _ => parts
        };
        
        return filteredParts.ToList();
    }

    private static void CachePartFields(LiteDatabase db, Part part)
    {
        // PartType
        
        // Range
        // PartName
        // PackageType
        // ValueUnit
        // Location
        // Manufacturer
    }
}