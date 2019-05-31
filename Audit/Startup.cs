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

namespace Audit
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).
                AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            StaticConfig = Configuration;
        }

        public static IConfiguration StaticConfig { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);                 
            services.Configure<Utils.EnvironmentConfig>(Configuration);
            if (Configuration["EUREKA_URL"] != null && Configuration["SERVER_URL"] != null)
            {
                Configuration["eureka:client:serviceUrl"] = Configuration["EUREKA_URL"];
                Configuration["eureka:instance:hostName"] = Configuration["SERVER_URL"];
            }
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

            var repo = new Audit.Repository.AuditRepository(envConfig);
            var isUp = false;
            while (!isUp)
            {
                isUp = repo.IsUp().Result;
            }
            repo.CreateDd();

        }
    }
}
