using LiteDB;

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

    public LiteDatabase GetDatabase() => new(DbPath);
}


