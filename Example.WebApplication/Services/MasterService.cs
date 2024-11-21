namespace Example.WebApplication.Services;

using Example.WebApplication.Models;

using Smart.Data;
using Smart.Data.Mapper;

public sealed class MasterService
{
    private IDbProvider Provider { get; }

    public MasterService(IDbProvider provider)
    {
        Provider = provider;
    }

    public IList<ItemEntity> QueryItemList()
    {
        return Provider.Using(static con => con.QueryList<ItemEntity>("SELECT * FROM Item ORDER BY Id"));
    }
}
