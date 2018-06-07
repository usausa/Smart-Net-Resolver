namespace Example.WebApplication.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using Example.WebApplication.Models;

    using Smart.Data;

    public class CharacterService
    {
        private IConnectionFactory ConnectionFactory { get; }

        public CharacterService(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public IList<CharacterEntity> QueryCharacterList()
        {
            return ConnectionFactory.Using(
                con => con.Query<CharacterEntity>("SELECT * FROM Character ORDER BY Id", buffered: false).ToList());
        }
    }
}
