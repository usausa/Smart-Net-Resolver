namespace Example.WebApplication
{
    using System.IO;

    using Example.WebApplication.Services;
    using Example.WebApplication.Settings;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Data.Sqlite;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Smart.Data;
    using Smart.Data.Mapper;
    using Smart.Resolver;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Directory.SetCurrentDirectory(env.ContentRootPath);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddControllersAsServices()
                .AddViewComponentsAsServices()
                .AddTagHelpersAsServices();

            services.Configure<ProfileSettings>(Configuration.GetSection("ProfileSettings"));
        }

        public void ConfigureContainer(ResolverConfig config)
        {
            var connectionStringMaster = Configuration.GetConnectionString("Master");
            config
                .Bind<IDbProvider>()
                .ToConstant(new DelegateDbProvider(() => new SqliteConnection(connectionStringMaster)))
                .Named("Master");
            var connectionStringCharacter = Configuration.GetConnectionString("Character");
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

            // Prepare database
            SetupMasterDatabase(connectionStringMaster);
            SetupCharacterDatabase(connectionStringCharacter);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void SetupMasterDatabase(string connectionString)
        {
            using var con = new SqliteConnection(connectionString);
            con.Execute("CREATE TABLE IF NOT EXISTS Item (Id int PRIMARY KEY, Name text, Price int)");
            con.Execute("DELETE FROM Item");
            con.Execute("INSERT INTO Item (Id, Name, Price) VALUES (1, 'Item-1', 100)");
            con.Execute("INSERT INTO Item (Id, Name, Price) VALUES (2, 'Item-2', 200)");
            con.Execute("INSERT INTO Item (Id, Name, Price) VALUES (3, 'Item-3', 300)");
        }

        private static void SetupCharacterDatabase(string connectionString)
        {
            using var con = new SqliteConnection(connectionString);
            con.Execute("CREATE TABLE IF NOT EXISTS Character (Id int PRIMARY KEY, Name text, Level int)");
            con.Execute("DELETE FROM Character");
            con.Execute("INSERT INTO Character (Id, Name, Level) VALUES (1, 'Character-1', 43)");
            con.Execute("INSERT INTO Character (Id, Name, Level) VALUES (2, 'Character-2', 65)");
            con.Execute("INSERT INTO Character (Id, Name, Level) VALUES (3, 'Character-3', 27)");
        }
    }
}
