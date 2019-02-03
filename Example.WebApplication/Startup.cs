namespace Example.WebApplication
{
    using Dapper;

    using Example.WebApplication.Services;
    using Example.WebApplication.Settings;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.Sqlite;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Smart.Data;
    using Smart.Resolver;
    using Smart.Resolver.Configs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<ProfileSettings>(Configuration.GetSection("ProfileSettings"));
        }

        public void ConfigureContainer(ResolverConfig config)
        {
            // TODO
            //config.UseMissingHandler<ControllerMissingHandler>();
            //config.UseMissingHandler<ViewComponentMissingHandler>();
            //config.Bind<IControllerActivator>().To<SmartResolverControllerActivator>().InSingletonScope();
            //config.Bind<IViewComponentActivator>().To<SmartResolverViewComponentActivator>().InSingletonScope();

            var connectionStringMaster = Configuration.GetConnectionString("Master");
            config
                .Bind<IConnectionFactory>()
                .ToConstant(new CallbackConnectionFactory(() => new SqliteConnection(connectionStringMaster)))
                .Named("Master");
            var connectionStringCharacter = Configuration.GetConnectionString("Character");
            config
                .Bind<IConnectionFactory>()
                .ToConstant(new CallbackConnectionFactory(() => new SqliteConnection(connectionStringCharacter)))
                .Named("Character");

            config
                .Bind<MasterService>()
                .ToSelf()
                .InSingletonScope()
                .WithConstructorArgument("connectionFactory", kernel => kernel.Get<IConnectionFactory>("Master"));
            config
                .Bind<CharacterService>()
                .ToSelf()
                .InSingletonScope()
                .WithConstructorArgument("connectionFactory", kernel => kernel.Get<IConnectionFactory>("Character"));

            config
                .Bind<MetricsManager>()
                .ToSelf()
                .InSingletonScope();

            config
                .Bind<ScopedObject>()
                .ToSelf()
                .InContextScope();

            // Prepare database
            SetupMasterDatabase(connectionStringMaster);
            SetupCharacterDatabase(connectionStringCharacter);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void SetupMasterDatabase(string connectionString)
        {
            using (var con = new SqliteConnection(connectionString))
            {
                con.Execute("CREATE TABLE IF NOT EXISTS Item (Id int PRIMARY KEY, Name text, Price int)");
                con.Execute("DELETE FROM Item");
                con.Execute("INSERT INTO Item (Id, Name, Price) VALUES (1, 'Item-1', 100)");
                con.Execute("INSERT INTO Item (Id, Name, Price) VALUES (2, 'Item-2', 200)");
                con.Execute("INSERT INTO Item (Id, Name, Price) VALUES (3, 'Item-3', 300)");
            }
        }

        private static void SetupCharacterDatabase(string connectionString)
        {
            using (var con = new SqliteConnection(connectionString))
            {
                con.Execute("CREATE TABLE IF NOT EXISTS Character (Id int PRIMARY KEY, Name text, Level int)");
                con.Execute("DELETE FROM Character");
                con.Execute("INSERT INTO Character (Id, Name, Level) VALUES (1, 'Character-1', 43)");
                con.Execute("INSERT INTO Character (Id, Name, Level) VALUES (2, 'Character-2', 65)");
                con.Execute("INSERT INTO Character (Id, Name, Level) VALUES (3, 'Character-3', 27)");
            }
        }
    }
}
