using LiteDB.Async;
using partsbin.BusinessLogic.Models;
using Polly;

namespace partsbin.BusinessLogic.Services;

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
    private string ConnectionString => _isProduction
        ? "Filename=/data/partsbin.db; Connection=Shared;"
        : "Filename=./partsbin-dev.db; Connection=Shared";

    public async Task<LiteDatabaseAsync> GetDatabase()
    {
        var policy = Policy
            .Handle<IOException>()
            .WaitAndRetry(5, _ => TimeSpan.FromSeconds(1));
        var db = policy.Execute(() => new LiteDatabaseAsync(ConnectionString));

        // set up some indices
        var partCollection = db.GetCollection<Part>();
        await partCollection.EnsureIndexAsync(x => x.Location);    
        await partCollection.EnsureIndexAsync(x => x.Manufacturer);    
        await partCollection.EnsureIndexAsync(x => x.PartName);    
        await partCollection.EnsureIndexAsync(x => x.PartType);    
        await partCollection.EnsureIndexAsync(x => x.Range);    
        await partCollection.EnsureIndexAsync(x => x.PartNumber);  

        var equipmentCollection = db.GetCollection<Equipment>();
        await equipmentCollection.EnsureIndexAsync(x => x.EquipmentName);  
        await equipmentCollection.EnsureIndexAsync(x => x.EquipmentType);  
        await equipmentCollection.EnsureIndexAsync(x => x.Manufacturer);  
        await equipmentCollection.EnsureIndexAsync(x => x.ModelNumber);  
        await equipmentCollection.EnsureIndexAsync(x => x.Location);

        return db;
    }
}


