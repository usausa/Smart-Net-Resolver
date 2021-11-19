namespace Example.WebApplication.Controllers;

using System;

using Example.WebApplication.Services;

using Microsoft.AspNetCore.Mvc;

public class ScopedController : Controller
{
    private ScopedObject ScopedObject { get; }

    public ScopedController(ScopedObject scopedObject)
    {
        ScopedObject = scopedObject;
    }

    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
    public IActionResult Index([FromServices] ScopedObject scopedObject)
    {
        if (ScopedObject != scopedObject)
        {
            throw new InvalidOperationException("Scoped object unmatch.");
        }

        return View(ScopedObject);
    }
}
