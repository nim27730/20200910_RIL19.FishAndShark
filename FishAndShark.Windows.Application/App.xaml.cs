using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RIL19.FishAndShark.Windows.Application.ViewModels;
using Refit;
using RIL19.FishAndShark.Windows.Application.Services;
using Serilog;

namespace RIL19.FishAndShark.Windows.Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .UseSerilog(((context, configuration) => configuration
                        .MinimumLevel.Verbose()
                        .WriteTo.Console()
                        .WriteTo.File(@"c:\temp\logfishandshark.log")
                    ))
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .Build();

            ServicesLocator.Initialize(_host.Services);
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<MainWindowViewModel>();

            var refitSettings = new RefitSettings()
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(
                    new JsonSerializerSettings()
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        TypeNameHandling = TypeNameHandling.None
                    })
            };

            services.AddRefitClient<IAquariumApi>(refitSettings).ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(@"https://localhost:44382/");
                });

            services.AddSingleton((service) =>
           {
               var connection = new HubConnectionBuilder()
                   .ConfigureLogging((loggingBuilder) =>
                   {
                       loggingBuilder.AddSerilog();
                   })
                   .WithUrl(@"https://localhost:44382/hubs/aquarium")
                   //.WithAutomaticReconnect()
                   .Build();
               connection.Closed += async (ErrorContext) =>
               {
                   await Task.Delay(5000);
                   await connection.StartAsync();
               };
               connection.StartAsync();
               return connection;
           });
        }
    }
}
