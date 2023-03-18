using System.Text.Encodings.Web;
using partsbin.Models;

namespace partsbin.Services;

public interface ISupplierService
{
    IEnumerable<(string name, string url)> GetNamesAndUrls();
}


public class SupplierService : ISupplierService
{
    private readonly IDbFactory _dbFactory;

    public SupplierService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    private struct NameAndUrl
    {
        public required string Name;
        public required string Url;
    }

    private class NamesAndUrlsEqualityComparer : IEqualityComparer<NameAndUrl>
    {
        public bool Equals(NameAndUrl x, NameAndUrl y)
        {
            return x.Name == y.Name && x.Url == y.Url;
        }

        public int GetHashCode(NameAndUrl obj)
        {
            return HashCode.Combine(obj.Name, obj.Url);
        }
    }
    
    public IEnumerable<(string name, string url)> GetNamesAndUrls()
    {
        using var db = _dbFactory.GetDatabase();

        // Big ol query here, beware!
        return db.GetCollection<Part>()
            .Query()
            .ToList()
            .SelectMany(x => x.Suppliers)
            .Select(x => new NameAndUrl
            {
                Name = x.Name,
                Url = x.Url
            })
            .Distinct(new NamesAndUrlsEqualityComparer())
            .Select(x => (name: x.Name, url: x.Url));
    }
}