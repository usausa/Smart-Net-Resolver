#pragma warning disable CA1812
using Example.WebApplication;
using Example.WebApplication.Services;
using Example.WebApplication.Settings;

using Microsoft.Data.Sqlite;

using Smart.Data;
using Smart.Resolver;

// Configure builder
var builder = WebApplication.CreateBuilder(args);

// Custom service provider
builder.Host.UseServiceProviderFactory(new SmartServiceProviderFactory());

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddControllersAsServices()
    .AddViewComponentsAsServices()
    .AddTagHelpersAsServices();

builder.Services.Configure<ProfileSettings>(builder.Configuration.GetSection("ProfileSettings"));

var connectionStringMaster = builder.Configuration.GetConnectionString("Master");
var connectionStringCharacter = builder.Configuration.GetConnectionString("Character");

// Custom service provider
builder.Host.ConfigureContainer<ResolverConfig>(config =>
{
    config
        .Bind<IDbProvider>()
        .ToConstant(new DelegateDbProvider(() => new SqliteConnection(connectionStringMaster)))
        .Named("Master");
    config
        .Bind<IDbProvider>()
        .ToConstant(new DelegateDbProvider(() => new SqliteConnection(connectionStringCharacter)))
        .Named("Character");

    config
        .Bind<MasterService>()
        .ToSelf()
        .InSingletonScope()
        .WithConstructorArgument("provider", kernel => kernel.Get<IDbProvider>("Master"));
    config
        .Bind<CharacterService>()
        .ToSelf()
        .InSingletonScope()
        .WithConstructorArgument("provider", kernel => kernel.Get<IDbProvider>("Character"));

    config
        .Bind<MetricsManager>()
        .ToSelf()
        .InSingletonScope();

    config
        .Bind<ScopedObject>()
        .ToSelf()
        .InContainerScope();
});

// Configure
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Data initialize
DatabaseInitializer.SetupMasterDatabase(connectionStringMaster!);
DatabaseInitializer.SetupCharacterDatabase(connectionStringCharacter!);

app.Run();
