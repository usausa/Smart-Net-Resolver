namespace Example.WebApplication.Controllers;

using Example.WebApplication.Models;
using Example.WebApplication.Services;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public sealed class ItemController : Controller
{
    private MasterService MasterService { get; }

    public ItemController(MasterService masterService)
    {
        MasterService = masterService;
    }

    [HttpGet]
    public IEnumerable<ItemEntity> Get()
    {
        return MasterService.QueryItemList();
    }
}
