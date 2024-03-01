using Chromely;
using Chromely.Core;
using Chromely.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace BasicHTMLDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var config = DefaultConfiguration.CreateForRuntimePlatform();
            config.StartUrl = "local://index.html";

            AppBuilder
            .Create()
            .UseConfig<DefaultConfiguration>(config)
            .UseApp<DemoApp>()
            .Build()
            .Run(args);
        }
    }

    public class DemoApp : ChromelyBasicApp
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddLogging(configure => configure.AddConsole());

            RegisterControllerAssembly(services, typeof(DemoApp).Assembly);
        }
    }
}
