using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Services.User
{
    using User.Data.Repository;
    using User.Processors;
    using Autofac;
    using Autofac.Extras.NLog;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using NLog;
    using NLog.Extensions.Logging;
    using NLog.Web;
    using RawRabbit.Configuration;
    using RawRabbit.DependencyInjection.Autofac;
    using RawRabbit.Logging;
    using RawRabbit.vNext;
    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public static string UserDbConnectionString { get; private set; }
        public static RawRabbitConfiguration RawRabbitConfiguration { get; private set; }


        private IHostingEnvironment _env;

        public IConfigurationRoot Configuration { get; }

        public IContainer Container { get; }

        private NLog.ILogger _logger;


        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
            }

            UserDbConnectionString = Configuration.GetConnectionString("UserDb");
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            // Add framework services.
            services.AddMvc()
            .AddJsonOptions(config =>
            {
                config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                config.SerializerSettings.Error = (object sender, ErrorEventArgs args) =>
                {
                    _logger.Error(args.ErrorContext.Error);
                };
            });


            services.AddRawRabbit(
                    cfg => cfg.AddJsonFile("rawrabbit.json"),
                    ioc => ioc.AddSingleton<ILoggerFactory, RawRabbit.Logging.NLog.LoggerFactory>()
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "User API", Version = "v1" });
            });
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<NLogModule>();
            builder.RegisterRawRabbit();
            builder.RegisterType<UserRepository>().As<IUserRepository>().WithParameter("connectionString", UserDbConnectionString);
            builder.RegisterType<UserProcessor>().As<IUserProcessor>()
                .WithParameter("UserRepository", new UserRepository(UserDbConnectionString))
                .WithParameter("logger", NLog.LogManager.GetCurrentClassLogger());
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            //loggerFactory.AddNLog();
            app.AddNLogWeb();
            env.ConfigureNLog("nlog.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // TODO:  Create
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMvc();

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserProfile API V1");
            });
        }

    }
}
