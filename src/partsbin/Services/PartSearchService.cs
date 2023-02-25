using partsbin.Models;

namespace partsbin.Services;

public interface IPartSearchService
{
    IEnumerable<Part> Search(string phrase);
}

public class PartSearchService : IPartSearchService
{
    private readonly IDbFactory _dbFactory;

    public PartSearchService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public IEnumerable<Part> Search(string phrase)
    {
        using var db = _dbFactory.GetDatabase();

        phrase = phrase.ToLower();

        return Array.Empty<Part>();
    }
}