using partsbin.Models;
using partsbin.Services.Results;

namespace partsbin.Services;

public interface IPartFieldService
{
    IEnumerable<PartTypeAndRangesResult> GetPartTypesAndRanges();
    IEnumerable<string> GetUniquePartTypes();
    IEnumerable<(string partType, int quantity)> GetUniquePartTypesAndCounts();
    IEnumerable<string> GetUniqueRanges();
    IEnumerable<(string range, int quantity)> GetUniqueRangesAndCounts();
    IEnumerable<string> GetUniquePackageTypes();
    IEnumerable<string> GetUniquePartNames();
    IEnumerable<(string partName, int quantity)> GetUniquePartNamesAndCounts();
    IEnumerable<string> GetUniqueValueUnits();
    IEnumerable<string> GetUniqueManufacturers();
    IEnumerable<string> GetUniqueLocations();
}

public class PartFieldService : IPartFieldService
{
    private readonly IDbFactory _dbFactory;

    public PartFieldService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }
    
    public IEnumerable<PartTypeAndRangesResult> GetPartTypesAndRanges()
    {
        using var db = _dbFactory.GetDatabase();

        var result = db.GetCollection<Part>()
            .Query()
            .Select(x => new
            {
                x.PartType,
                x.Range
            })
            .ToList()
            .GroupBy(x => x)
            .Select(x => new PartTypeAndRangesResult
            {
                PartType = x.Key.PartType,
                Range = x.Key.Range,
                Count = x.Count()
            });

        return result;
    }

    public IEnumerable<string> GetUniquePartTypes()
    {
        return GetUniquePartTypesAndCounts()
            .Select(x => x.partType)
            .Distinct();
    }
    
    public IEnumerable<(string partType, int quantity)> GetUniquePartTypesAndCounts()
    {
        using var db = _dbFactory.GetDatabase();
        
        var result = db.GetCollection<Part>()
            .Query()
            .Where(x => x.PartType != null && x.PartType != string.Empty)
            .Select(x => new { x.PartType, x.Quantity })
            .ToList()
            .GroupBy(x => x.PartType, x => x.Quantity)
            .Select(x => new
            {
                PartType = x.Key, 
                Quantity = x.Sum(),
            });

        return result
            .Select(x => (x.PartType!, x.Quantity));
    }

    public IEnumerable<string> GetUniqueRanges()
    {
        return GetUniqueRangesAndCounts()
            .Select(x => x.range)
            .Distinct();
    }

    public IEnumerable<(string range, int quantity)> GetUniqueRangesAndCounts()
    {
        using var db = _dbFactory.GetDatabase();
        
        var result = db.GetCollection<Part>()
            .Query()
            .Where(x => x.Range != null && x.Range != string.Empty)
            .Select(x => new { x.Range, x.Quantity})
            .ToList()
            .GroupBy(x => x.Range, x => x.Quantity)
            .Select(x =>  new
            {
                Range =x.Key,
                Quantity = x.Sum()
            });

        return result.Select(x => (x.Range!, x.Quantity));
    }

    public IEnumerable<string> GetUniquePartNames()
    {
        return GetUniquePartNamesAndCounts()
            .Select(x => x.partName)
            .Distinct();
    }

    public IEnumerable<(string partName, int quantity)> GetUniquePartNamesAndCounts()
    {
        using var db = _dbFactory.GetDatabase();
        
        var result = db.GetCollection<Part>()
            .Query()
            .Where(x => x.PartName != null && x.PartName != string.Empty)
            .Select(x => new { x.PartName, x.Quantity})
            .ToList()
            .GroupBy(x => x.PartName, x=>x.Quantity)
            .Select(x => new
            {
                PartType = x.Key,
                Quantity = x.Sum()
            });

        return result.Select(x => (x.PartType!, x.Quantity));
    }

    public IEnumerable<string> GetUniquePackageTypes()
    {
        using var db = _dbFactory.GetDatabase();
        
        var result = db.GetCollection<Part>()
            .Query()
            .Where(x => x.PackageType != null && x.PackageType != string.Empty)
            .Select(x => x.PackageType)
            .ToList()
            .Select(x => x!)
            .Distinct();

        return result;
    }
    
    public IEnumerable<string> GetUniqueValueUnits()
    {
        using var db = _dbFactory.GetDatabase();
        
        var result = db.GetCollection<Part>()
            .Query()
            .Where(x => x.ValueUnit != null && x.ValueUnit != string.Empty)
            .Select(x => x.ValueUnit)
            .ToList()
            .Select(x => x!)
            .Distinct();

        return result;
    }
    
    public IEnumerable<string> GetUniqueManufacturers()
    {
        using var db = _dbFactory.GetDatabase();
        
        var result = db.GetCollection<Part>()
            .Query()
            .Where(x => x.Manufacturer != null && x.Manufacturer != string.Empty)
            .Select(x => x.Manufacturer)
            .ToList()
            .Select(x => x!)
            .Distinct();

        return result;
    }
    
    public IEnumerable<string> GetUniqueLocations()
    {
        using var db = _dbFactory.GetDatabase();
        
        var result = db.GetCollection<Part>()
            .Query()
            .Where(x => x.Location != null && x.Location != string.Empty)
            .Select(x => x.Location)
            .ToList()
            .Select(x => x!)
            .Distinct();

        return result;
    }
}