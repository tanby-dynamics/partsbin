using LiteDB;
using partsbin.Models;

namespace partsbin.Services;

public interface IDbFactory
{
    LiteDatabase GetDatabase();
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

    public LiteDatabase GetDatabase()
    {
        var db = new LiteDatabase(DbPath);

        // set up some indices
        var partCollection = db.GetCollection<Part>();
        partCollection.EnsureIndex(x => x.Location);    
        partCollection.EnsureIndex(x => x.Manufacturer);    
        partCollection.EnsureIndex(x => x.PartName);    
        partCollection.EnsureIndex(x => x.PartType);    
        partCollection.EnsureIndex(x => x.Range);    
        partCollection.EnsureIndex(x => x.PartNumber);    

        return db;
    }
}


