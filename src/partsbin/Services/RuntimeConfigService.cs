using System.Globalization;
using partsbin.Models;

namespace partsbin.Services;

public interface IRuntimeConfigService
{
    RuntimeConfig GetRuntimeConfig();
    void SetRuntimeConfig(RuntimeConfig config);
}

public class RuntimeConfigService : IRuntimeConfigService
{
    private readonly IDbFactory _dbFactory;

    public RuntimeConfigService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public RuntimeConfig GetRuntimeConfig()
    {
        using var db = _dbFactory.GetDatabase();
        var config = db.GetCollection<RuntimeConfig>()
            .Query()
            .SingleOrDefault();

        if (config is not null) return config;
        
        var newConfig = new RuntimeConfig
        {
            CultureName = CultureInfo.CurrentCulture.Name
        };
        
        SetRuntimeConfig(newConfig);

        return newConfig;
    }

    public void SetRuntimeConfig(RuntimeConfig config)
    {
        var db = _dbFactory.GetDatabase();
        var collection = db.GetCollection<RuntimeConfig>();

        collection.DeleteAll();
        collection.Insert(config);
    }
}