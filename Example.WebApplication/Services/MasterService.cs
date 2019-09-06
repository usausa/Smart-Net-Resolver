namespace Example.WebApplication.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using Example.WebApplication.Models;

    using Smart.Data;

    public class MasterService
    {
        private IDbProvider Provider { get; }

        public MasterService(IDbProvider provider)
        {
            Provider = provider;
        }

        public IList<ItemEntity> QueryItemList()
        {
            return Provider.Using(
                con => con.Query<ItemEntity>("SELECT * FROM Item ORDER BY Id", buffered: false).ToList());
        }
    }
}
