namespace Example.WebApplication.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using Example.WebApplication.Models;

    using Smart.Data;

    public class MasterService
    {
        private IConnectionFactory ConnectionFactory { get; }

        public MasterService(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public IList<ItemEntity> QueryItemList()
        {
            return ConnectionFactory.Using(
                con => con.Query<ItemEntity>("SELECT * FROM Item ORDER BY Id", buffered: false).ToList());
        }
    }
}
