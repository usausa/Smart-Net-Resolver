namespace Example.WebApplication.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Dapper;

    using Example.WebApplication.Models;

    using Smart.Data;

    public class CharacterService
    {
        private IDbProvider Provider { get; }

        public CharacterService(IDbProvider provider)
        {
            Provider = provider;
        }

        public IList<CharacterEntity> QueryCharacterList()
        {
            return Provider.Using(
                con => con.Query<CharacterEntity>("SELECT * FROM Character ORDER BY Id", buffered: false).ToList());
        }
    }
}
