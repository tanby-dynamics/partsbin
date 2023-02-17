using partsbin.Models;
using partsbin.Services.Results;

namespace partsbin.Services;

public interface IPartTypeService
{
    IEnumerable<PartTypeAndRangesResult> GetPartTypesAndRanges();
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
}