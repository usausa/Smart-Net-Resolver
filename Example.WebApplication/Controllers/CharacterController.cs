namespace Example.WebApplication.Controllers;

using Example.WebApplication.Models;
using Example.WebApplication.Services;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class CharacterController : Controller
{
    private CharacterService CharacterService { get; }

    public CharacterController(CharacterService characterService)
    {
        CharacterService = characterService;
    }

    [HttpGet]
    public IEnumerable<CharacterEntity> Get()
    {
        return CharacterService.QueryCharacterList();
    }
}
