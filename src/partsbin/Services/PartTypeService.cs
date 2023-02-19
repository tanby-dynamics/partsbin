using partsbin.Models;
using partsbin.Services.Results;

namespace partsbin.Services;

public interface IPartTypeService
{
    IEnumerable<PartTypeAndRangesResult> GetPartTypesAndRanges();
    IEnumerable<string> GetUniquePartTypes();
    IEnumerable<string> GetUniqueRanges();
}

public class PartTypeService : IPartTypeService
{
    private readonly IDbFactory _dbFactory;

    public PartTypeService(IDbFactory dbFactory)
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
        using var db = _dbFactory.GetDatabase();
        
        var result = db.GetCollection<Part>()
            .Query()
            .Where(x => x.PartType != null && x.PartType != string.Empty)
            .Select(x => x.PartType)
            .ToList()
            .Select(x => x!)
            .Distinct();

        return result;
    }
    
    public IEnumerable<string> GetUniqueRanges()
    {
        using var db = _dbFactory.GetDatabase();
        
        var result = db.GetCollection<Part>()
            .Query()
            .Where(x => x.Range != null && x.Range != string.Empty)
            .Select(x => x.Range)
            .ToList()
            .Select(x => x!)
            .Distinct();

        return result;
    }
}