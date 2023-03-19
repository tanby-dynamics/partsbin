using partsbin.Models;
using partsbin.Services.Results;

namespace partsbin.Services;

public interface IPartFieldService
{
    Task<IEnumerable<PartTypeAndRangesResult>> GetPartTypesAndRanges();
    Task<IEnumerable<string>> GetUniquePartTypes();
    Task<IEnumerable<(string partType, int quantity)>> GetUniquePartTypesAndCounts();
    Task<IEnumerable<string>> GetUniqueRanges();
    Task<IEnumerable<(string range, int quantity)>> GetUniqueRangesAndCounts();
    Task<IEnumerable<string>> GetUniquePackageTypes();
    Task<IEnumerable<string>> GetUniquePartNames();
    Task<IEnumerable<(string partName, int quantity)>> GetUniquePartNamesAndCounts();
    Task<IEnumerable<string>> GetUniqueValueUnits();
    Task<IEnumerable<(string manufacturer, int quantity)>> GetUniqueManufacturersAndCounts();
    Task<IEnumerable<string>> GetUniqueManufacturers();
    Task<IEnumerable<string>> GetUniqueLocations();
    Task<IEnumerable<(string location, int quantity)>> GetUniqueLocationsAndCounts();
    Task<IEnumerable<(string partNumber, int quantity)>> GetUniquePartNumbersAndCounts();
    Task<IEnumerable<string>> GetUniquePartNumbers();
}

public class PartFieldService : IPartFieldService
{
    private readonly IDbFactory _dbFactory;

    public PartFieldService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<IEnumerable<PartTypeAndRangesResult>> GetPartTypesAndRanges()
    {
        using var db = await _dbFactory.GetDatabase();

        var parts = await db.GetCollection<Part>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Select(x => new
            {
                x.PartType,
                x.Range
            })
            .ToListAsync();
        var result = parts
            .GroupBy(x => x)
            .Select(x => new PartTypeAndRangesResult
            {
                PartType = x.Key.PartType,
                Range = x.Key.Range,
                Count = x.Count()
            });

        return result;
    }

    public async Task<IEnumerable<string>> GetUniquePartTypes()
    {
        return (await GetUniquePartTypesAndCounts())
            .Select(x => x.partType)
            .Distinct();
    }

    public async Task<IEnumerable<(string partType, int quantity)>> GetUniquePartTypesAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var parts = await db.GetCollection<Part>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.PartType != null && x.PartType != string.Empty)
            .Select(x => new { x.PartType, x.Quantity })
            .ToListAsync();
     
        return parts
            .GroupBy(x => x.PartType, x => x.Quantity)
            .Select(x => new
            {
                PartType = x.Key,
                Quantity = x.Sum(),
            })
            .Select(x => (x.PartType!, x.Quantity));
    }

    public async Task<IEnumerable<string>> GetUniqueRanges()
    {
        return (await GetUniqueRangesAndCounts())
            .Select(x => x.range)
            .Distinct();
    }

    public async Task<IEnumerable<(string range, int quantity)>> GetUniqueRangesAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var ranges = await db.GetCollection<Part>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.Range != null && x.Range != string.Empty)
            .Select(x => new { x.Range, x.Quantity })
            .ToListAsync();

        return ranges.GroupBy(x => x.Range, x => x.Quantity)
            .Select(x => new
            {
                Range = x.Key,
                Quantity = x.Sum()
            })
            .Select(x => (x.Range!, x.Quantity));
    }

    public async Task<IEnumerable<string>> GetUniquePartNames()
    {
        return (await GetUniquePartNamesAndCounts())
            .Select(x => x.partName)
            .Distinct();
    }

    public async Task<IEnumerable<(string partName, int quantity)>> GetUniquePartNamesAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var partNames = await db.GetCollection<Part>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.PartName != null && x.PartName != string.Empty)
            .Select(x => new { x.PartName, x.Quantity })
            .ToListAsync();

        return partNames
            .GroupBy(x => x.PartName, x => x.Quantity)
            .Select(x => new
            {
                PartType = x.Key,
                Quantity = x.Sum()
            })
            .Select(x => (x.PartType!, x.Quantity));
    }

    public async Task<IEnumerable<string>> GetUniquePackageTypes()
    {
        using var db = await _dbFactory.GetDatabase();

        var packageTypes = await db.GetCollection<Part>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.PackageType != null && x.PackageType != string.Empty)
            .Select(x => x.PackageType)
            .ToListAsync();

        return packageTypes
            .Select(x => x!)
            .Distinct();
    }

    public async Task<IEnumerable<string>> GetUniqueValueUnits()
    {
        using var db = await _dbFactory.GetDatabase();

        var valueUnits = await db.GetCollection<Part>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.ValueUnit != null && x.ValueUnit != string.Empty)
            .Select(x => x.ValueUnit)
            .ToListAsync();

        return valueUnits
            .Select(x => x!)
            .Distinct();
    }

    public async Task<IEnumerable<string>> GetUniqueManufacturers()
    {
        return (await GetUniqueManufacturersAndCounts())
            .Select(x => x.manufacturer)
            .Distinct();
    }

    public async Task<IEnumerable<string>> GetUniquePartNumbers()
    {
        return (await GetUniquePartNumbersAndCounts())
            .Select(x => x.partNumber)
            .Distinct();
    }

    public async Task<IEnumerable<(string manufacturer, int quantity)>> GetUniqueManufacturersAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var manufacturers = await db.GetCollection<Part>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.Manufacturer != null && x.Manufacturer != string.Empty)
            .Select(x => new { x.Manufacturer, x.Quantity })
            .ToListAsync();

        return manufacturers
            .GroupBy(x => x.Manufacturer, x => x.Quantity)
            .Select(x => new
            {
                Manufacturer = x.Key,
                Quantity = x.Sum()
            })
            .Select(x => (x.Manufacturer!, x.Quantity));
    }

    public async Task<IEnumerable<string>> GetUniqueLocations()
    {
        return (await GetUniqueLocationsAndCounts())
            .Select(x => x.location)
            .Distinct();
    }

    public async Task<IEnumerable<(string partNumber, int quantity)>> GetUniquePartNumbersAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var partNumbers = await db.GetCollection<Part>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.PartNumber != null && x.PartNumber != string.Empty)
            .Select(x => new { x.PartNumber, x.Quantity })
            .ToListAsync();

        return partNumbers
            .GroupBy(x => x.PartNumber, x => x.Quantity)
            .Select(x => new
            {
                PartNumber = x.Key,
                Quantity = x.Sum()
            })
            .Select(x => (x.PartNumber!, x.Quantity));
    }

    public async Task<IEnumerable<(string location, int quantity)>> GetUniqueLocationsAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var locations = await db.GetCollection<Part>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.Location != null && x.Location != string.Empty)
            .Select(x => new { x.Location, x.Quantity })
            .ToListAsync();

        return locations
            .GroupBy(x => x.Location, x => x.Quantity)
            .Select(x => new
            {
                Location = x.Key,
                Quantity = x.Sum()
            })
            .Select(x => (x.Location!, x.Quantity));
    }
}