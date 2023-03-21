using System.Globalization;
using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Services;

public interface IRuntimeConfigService
{
    Task<RuntimeConfig> GetRuntimeConfig();
    Task SetRuntimeConfig(RuntimeConfig config);
}

public class RuntimeConfigService : IRuntimeConfigService
{
    private readonly IDbFactory _dbFactory;

    public RuntimeConfigService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<RuntimeConfig> GetRuntimeConfig()
    {
        using var db = await _dbFactory.GetDatabase();
        var config = await db.GetCollection<RuntimeConfig>()
            .Query()
            .SingleOrDefaultAsync();

        if (config is not null) return config;
        
        var newConfig = new RuntimeConfig
        {
            CultureName = CultureInfo.CurrentCulture.Name
        };
        
        await SetRuntimeConfig(newConfig);

        return newConfig;
    }

    public async Task SetRuntimeConfig(RuntimeConfig config)
    {
        var db = await _dbFactory.GetDatabase();
        var collection = db.GetCollection<RuntimeConfig>();

        await collection.DeleteAllAsync();
        await collection.InsertAsync(config);
    }
}