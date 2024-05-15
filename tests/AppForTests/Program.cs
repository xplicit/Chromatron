using System;
using System.IO;
using System.Text.Json;
using Chromatron;
using Chromatron.Browser;
using Chromatron.Core;
using Chromatron.Core.Configuration;
using Chromatron.Core.Host;
using Chromatron.Core.Logging;
using Chromatron.Core.Network;
using Microsoft.Extensions.Logging;

public class Program
{
    internal sealed class UnitTestWindow : Window
    {
        public UnitTestWindow(IChromatronNativeHost nativeHost, IChromatronConfiguration config,
            ChromatronHandlersResolver handlersResolver) : base(nativeHost, config, handlersResolver)
        {
            ConsoleMessage += ReceiveConsoleMessage;
        }

        private void ReceiveConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            if (e?.Message != null)
            {
#if DEBUG
                Logger.Instance.Log.LogInformation(e.Message);
#endif

                try
                {
                    var response = JsonSerializer.Deserialize<ChromatronResponse>(e.Message);
                    Environment.Exit(response.Status);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Logger.Instance.Log.LogError(ex, ex.Message);
#endif
                }
            }
        }
    }

    private static void Main(string[] args)
    {
        var config = DefaultConfiguration.CreateForRuntimePlatform();
        config.CefDownloadOptions = new CefDownloadOptions(autoDownload: true, silentDownload: true);
#if DEBUG
        //config.CustomSettings["logSeverity"] = "verbose";
#endif
        config.DebuggingMode = false;
        config.StartUrl = $"file:///{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "index.html")}";
        config.WindowOptions = new WindowOptions
        {
            Position = new WindowPosition(0, 0),
            Size = new WindowSize(0, 0),
            WindowState = WindowState.Minimize,
        };

        AppBuilder.Create([])
            .UseConfig<DefaultConfiguration>(config)
            .UseWindow<UnitTestWindow>()
            .UseApp<ChromatronBasicApp>()
            .Build()
            .Run();
    }
}