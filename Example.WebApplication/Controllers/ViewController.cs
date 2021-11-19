namespace Example.WebApplication.Controllers;

using Microsoft.AspNetCore.Mvc;

public class ViewController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
