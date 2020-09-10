using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RIL19.FishAndShark.API.Hubs;
using RIL19.FishAndShark.Mongodb;

namespace RIL19.FishAndShark.API
{
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
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                o.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            }); ;
            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = doc =>
                {
                    doc.Info.Version = "v1";
                    doc.Info.Title = "FishAndShark API";
                    doc.Info.Description = "REST API to serve data for FishAndShark applications";
                };
            });
            services.AddMongoDBRepository();
            services.AddSignalR();
            services.AddSingleton((service) =>
           {
               var connection = new HubConnectionBuilder()
                   .WithUrl("https://localhost:44382/hubs/aquarium")
                   .WithAutomaticReconnect()
                   .Build();
               connection.Closed += async (ErrorContext) =>
               {
                   await Task.Delay(5000);
                   await connection.StartAsync();
               };
               connection.StartAsync().GetAwaiter().GetResult();
               return connection;
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AquariumHub>("/hubs/aquarium");
            });

            // Register the Swagger generator and the Swagger UI
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
