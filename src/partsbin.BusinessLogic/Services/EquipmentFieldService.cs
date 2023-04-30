using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Services;

public interface IEquipmentFieldService
{
    Task<IEnumerable<(string equipmentType, int count)>> GetEquipmentTypesAndRanges();
    Task<IEnumerable<string>> GetUniqueEquipmentTypes();
    Task<IEnumerable<(string equipmentType, int quantity)>> GetUniqueEquipmentTypesAndCounts();
    Task<IEnumerable<string>> GetUniqueEquipmentNames();
    Task<IEnumerable<(string equipmentName, int quantity)>> GetUniqueEquipmentNamesAndCounts();
    Task<IEnumerable<(string manufacturer, int quantity)>> GetUniqueManufacturersAndCounts();
    Task<IEnumerable<string>> GetUniqueManufacturers();
    Task<IEnumerable<string>> GetUniqueLocations();
    Task<IEnumerable<(string location, int quantity)>> GetUniqueLocationsAndCounts();
    Task<IEnumerable<(string modelNumber, int quantity)>> GetUniqueModelNumbersAndCounts();
    Task<IEnumerable<string>> GetUniqueModelNumbers();
}

public class EquipmentFieldService : IEquipmentFieldService
{
    private readonly IDbFactory _dbFactory;

    public EquipmentFieldService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }
    
    public async Task<IEnumerable<(string equipmentType, int count)>> GetEquipmentTypesAndRanges()
    {
        using var db = await _dbFactory.GetDatabase();

        var equipment = await db.GetCollection<Equipment>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Select(x => x.EquipmentType)
            .ToListAsync();
        var result = equipment
            .GroupBy(x => x)
            .Select(x => (
                partType: x.Key,
                count: x.Count()
            ));

        return result;
    }

    public async Task<IEnumerable<string>> GetUniqueEquipmentTypes()
    {
        return (await GetUniqueEquipmentTypesAndCounts())
            .Select(x => x.equipmentType)
            .Distinct();
    }

    public async Task<IEnumerable<(string equipmentType, int quantity)>> GetUniqueEquipmentTypesAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var parts = await db.GetCollection<Equipment>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.EquipmentType != null && x.EquipmentType != string.Empty)
            .Select(x => new { x.EquipmentType, x.Quantity })
            .ToListAsync();
     
        return parts
            .GroupBy(x => x.EquipmentType, x => x.Quantity)
            .Select(x => new
            {
                PartType = x.Key,
                Quantity = x.Sum(),
            })
            .Select(x => (x.PartType!, x.Quantity));
    }

    public async Task<IEnumerable<string>> GetUniqueEquipmentNames()
    {
        return (await GetUniqueEquipmentNamesAndCounts())
            .Select(x => x.equipmentName)
            .Distinct();
    }

    public async Task<IEnumerable<(string equipmentName, int quantity)>> GetUniqueEquipmentNamesAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var equipmentNames = await db.GetCollection<Equipment>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.EquipmentName != null && x.EquipmentName != string.Empty)
            .Select(x => new { x.EquipmentName, x.Quantity })
            .ToListAsync();

        return equipmentNames
            .GroupBy(x => x.EquipmentName, x => x.Quantity)
            .Select(x => new
            {
                PartType = x.Key,
                Quantity = x.Sum()
            })
            .Select(x => (x.PartType!, x.Quantity));
    }

    public async Task<IEnumerable<string>> GetUniqueManufacturers()
    {
        return (await GetUniqueManufacturersAndCounts())
            .Select(x => x.manufacturer)
            .Distinct();
    }

    public async Task<IEnumerable<string>> GetUniqueModelNumbers()
    {
        return (await GetUniqueModelNumbersAndCounts())
            .Select(x => x.modelNumber)
            .Distinct();
    }

    public async Task<IEnumerable<(string manufacturer, int quantity)>> GetUniqueManufacturersAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var manufacturers = await db.GetCollection<Equipment>()
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

    public async Task<IEnumerable<(string modelNumber, int quantity)>> GetUniqueModelNumbersAndCounts()
    {
        using var db = await _dbFactory.GetDatabase();

        var partNumbers = await db.GetCollection<Equipment>()
            .Query()
            .Where(x => !x.IsDeleted)
            .Where(x => x.ModelNumber != null && x.ModelNumber != string.Empty)
            .Select(x => new { x.ModelNumber, x.Quantity })
            .ToListAsync();

        return partNumbers
            .GroupBy(x => x.ModelNumber, x => x.Quantity)
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

        var locations = await db.GetCollection<Equipment>()
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