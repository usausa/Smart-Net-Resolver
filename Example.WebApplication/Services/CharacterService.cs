namespace Example.WebApplication.Services;

using Example.WebApplication.Models;

using Smart.Data;
using Smart.Data.Mapper;

public sealed class CharacterService
{
    private IDbProvider Provider { get; }

    public CharacterService(IDbProvider provider)
    {
        Provider = provider;
    }

    public IList<CharacterEntity> QueryCharacterList()
    {
        return Provider.Using(static con => con.QueryList<CharacterEntity>("SELECT * FROM Character ORDER BY Id"));
    }
}
