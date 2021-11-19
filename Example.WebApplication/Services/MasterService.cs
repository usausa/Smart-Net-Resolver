namespace Example.WebApplication.Services;

using System.Collections.Generic;

using Example.WebApplication.Models;

using Smart.Data;
using Smart.Data.Mapper;

public class MasterService
{
    private IDbProvider Provider { get; }

    public MasterService(IDbProvider provider)
    {
        Provider = provider;
    }

    public IList<ItemEntity> QueryItemList()
    {
        return Provider.Using(con => con.QueryList<ItemEntity>("SELECT * FROM Item ORDER BY Id"));
    }
}
