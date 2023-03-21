using LiteDB.Async;
using partsbin.Services.Models;

namespace partsbin.Services.Services;

public interface IDbFactory
{
    Task<LiteDatabaseAsync> GetDatabase();
}

public class DbFactory : IDbFactory
{
    private readonly bool _isProduction;

    public DbFactory(bool isProduction)
    {
        _isProduction = isProduction;
    }
    private string DbPath => _isProduction
        ? "/data/partsbin.db"
        : "./partsbin-dev.db";

    public async Task<LiteDatabaseAsync> GetDatabase()
    {
        var db = new LiteDatabaseAsync(DbPath);
        
        // set up some indices
        var partCollection = db.GetCollection<Part>();
        await partCollection.EnsureIndexAsync(x => x.Location);    
        await partCollection.EnsureIndexAsync(x => x.Manufacturer);    
        await partCollection.EnsureIndexAsync(x => x.PartName);    
        await partCollection.EnsureIndexAsync(x => x.PartType);    
        await partCollection.EnsureIndexAsync(x => x.Range);    
        await partCollection.EnsureIndexAsync(x => x.PartNumber);    

        return db;
    }
}


