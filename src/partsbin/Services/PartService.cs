using LiteDB;
using partsbin.Models;

namespace partsbin.Services;

public interface IPartService
{
    Part AddPart(Part part);
    void UpdatePart(Part part);
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