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

        return part;
    }

    public void UpdatePart(Part part)
    {
        using var db = _dbFactory.GetDatabase();

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

        db.GetCollection<Part>()
            .DeleteMany(x => x.IsDeleted);
    }

    public Part Duplicate(Part source) => AddPart(source.DeepClone());
}