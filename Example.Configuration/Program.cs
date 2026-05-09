#pragma warning disable CA1303
using Example.Configuration.Services;

using Microsoft.Extensions.Configuration;

using Smart.Resolver;
using Smart.Resolver.Configuration;

// ------------------------------------------------------------------
// Sample 1: Load from JSON file
// ------------------------------------------------------------------
Console.WriteLine("==== Sample 1: Load from JSON file ====");
{
    var config = new ResolverConfig()
        .LoadJsonFile("resolver.json");

    using var resolver = config.ToResolver();
    var service = resolver.Get<MessageService>();
    service.PrintMessage("JSON");
}

Console.WriteLine();

// ------------------------------------------------------------------
// Sample 2: Load from XML file
// ------------------------------------------------------------------
Console.WriteLine("==== Sample 2: Load from XML file ====");
{
    var config = new ResolverConfig()
        .LoadXmlFile("resolver.xml");

    using var resolver = config.ToResolver();
    var service = resolver.Get<MessageService>();
    service.PrintMessage("XML");
}

Console.WriteLine();

// ------------------------------------------------------------------
// Sample 3: Load from appsettings.json via IConfiguration
// ------------------------------------------------------------------
Console.WriteLine("==== Sample 3: Load from appsettings.json via IConfiguration ====");
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    var definition = new ResolverDefinition();
    configuration.GetSection("Resolver").Bind(definition);

    var config = new ResolverConfig()
        .LoadDefinition(definition);

    using var resolver = config.ToResolver();
    var service = resolver.Get<MessageService>();
    service.PrintMessage("appsettings.json");
}

Console.WriteLine();

// ------------------------------------------------------------------
// Sample 4: Load from multiple files (JSON + XML)
// ------------------------------------------------------------------
Console.WriteLine("==== Sample 4: Load from multiple files (JSON + XML) ====");
{
    var config = new ResolverConfig()
        .LoadJsonFile("resolver-greeter.json")
        .LoadXmlFile("resolver-service.xml");

    using var resolver = config.ToResolver();
    var service = resolver.Get<MessageService>();
    service.PrintMessage("multiple files");
}

Console.WriteLine();

// ------------------------------------------------------------------
// Sample 5: Mix code registration and file loading
// ------------------------------------------------------------------
Console.WriteLine("==== Sample 5: Mix code registration and file loading ====");
{
    var config = new ResolverConfig();

    config.Bind<IGreeter>().To<HiGreeter>().InSingletonScope();

    config.LoadJsonFile("resolver.json");

    using var resolver = config.ToResolver();

    var greeter = resolver.Get<IGreeter>();
    Console.WriteLine($"Greeter (last-registered wins): {greeter.Greet("World")}");

    var service = resolver.Get<MessageService>();
    service.PrintMessage("mixed");
}
