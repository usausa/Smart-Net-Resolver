namespace Example.WebApplication.Controllers;

using Microsoft.AspNetCore.Mvc;

public sealed class ViewController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
