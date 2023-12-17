namespace Example.WebApplication.Controllers;

using Example.WebApplication.Infrastructure;
using Example.WebApplication.Services;

using Microsoft.AspNetCore.Mvc;

[MetricsFilter]
public sealed class FilterController : Controller
{
    public IActionResult Index([FromServices] MetricsManager metricsManager)
    {
        return View(metricsManager);
    }
}
