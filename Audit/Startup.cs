using Audit.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Steeltoe.Discovery.Client;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Audit
{
    public class Startup
    {
        public Startup(IHostingEnvironment env,ILogger<Startup> logger)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).
                AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            StaticConfig = Configuration;
            _logger = logger;
        }
        private static ILogger _logger { get; set; }
        public static IConfiguration StaticConfig { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.Configure<Utils.EnvironmentConfig>(Configuration);
            if (Configuration[Constants.eurekaUrl] != null && Configuration[Constants.serverUrl] != null)
            {
                Configuration[Constants.eurekaServiceUrl] = Configuration[Constants.eurekaUrl];
                Configuration[Constants.eurekaServiceHostName] = Configuration[Constants.serverUrl];
            }
            StaticConfig = AppConfiguration(StaticConfig);
            services.AddDiscoveryClient(Configuration);
            // Register the Swagger generator, defining 1 Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Audit API", Version = "v1" });

            });
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddDiscoveryClient(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        static public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<Utils.EnvironmentConfig> envConfig)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var rewriteOpts = new RewriteOptions()
                .AddRedirect("v2/api-docs", "api/audit/swagger/v1/swagger.json");
            app.UseRewriter(rewriteOpts);
            var basepath = "/";
            if (envConfig.Value.SWAGGER_PATH != null)
            {
                basepath = envConfig.Value.SWAGGER_PATH;
            }


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = basepath);
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    IDictionary<string, PathItem> paths = new Dictionary<string, PathItem>();
                    foreach (var path in swaggerDoc.Paths)
                    {
                        paths.Add(path.Key.Replace(basepath, "/"), path.Value);
                    }
                    swaggerDoc.Paths = paths;
                });
            });

            // Enable middleware to serve swagger-ui(HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("swagger/v1/swagger.json", "Audit API V1");
                //Serve swagger UI as the app's root
                options.RoutePrefix = string.Empty;
            });
            app.UseDiscoveryClient();

            app.UseDiscoveryClient();

            app.UseMvc();

            var repo = new Audit.Repository.AuditRepository(envConfig,_logger);
            var isUp = false;
            while (!isUp)
            {
                _logger.LogInformation("Checking for DB");
                System.Threading.Thread.Sleep(10000);
                isUp = repo.IsUp().Result;
            }
            repo.CreateDd();

        }

        static public IConfiguration AppConfiguration(IConfiguration Configuration)
        {
            var attributeToNotSave = new List<string>();
            var my_conf = new Services.ConfigService().Get().Result;
            _logger.LogInformation("Getting Configuration..");
            while (my_conf.Count == 0)
            {
                _logger.LogWarning("Fail to get Configuration. Re-attenting..");
                System.Threading.Thread.Sleep(10000);
                my_conf = new Services.ConfigService().Get().Result;
            }
            for (int i = 0; i < my_conf.Count; i++)
            {
                if (my_conf.ContainsKey(Constants.auditNotSave+":" + i))
                {
                    attributeToNotSave.Add(my_conf[Constants.auditNotSave + ":" + i]);
                    my_conf.Remove(Constants.auditNotSave + ":" + i);
                }
                else
                {
                    break;
                }
            }
            if (attributeToNotSave.Count > 0)
            {
                Configuration[Constants.auditNotSave] = string.Join(',', attributeToNotSave);
            }
            else
            {
                Configuration[Constants.auditNotSave] = "";
            }
            foreach (var con in my_conf)
            {
                Configuration[con.Key] = con.Value;
            }
            return Configuration;
        }
    }
}
