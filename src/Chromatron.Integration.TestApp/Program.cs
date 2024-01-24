// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using System;
using System.Diagnostics;
using Chromatron.Browser;
using Chromatron.Core;
using Chromatron.Core.Configuration;
using Chromatron.Core.Host;
using Chromatron.Core.Infrastructure;

// ReSharper disable UnusedParameter.Global

namespace Chromatron.Integration.TestApp
{
    /// <summary>
    /// 
    /// This is a minimal chromatron application to be used during integration tests.
    ///
    /// PLEASE NOTE:
    /// Due it is cross platform it MUST NOT reference PLATFORM SPECIFIC ASSEMBLIES.
    ///
    /// Use projects in Chromatron-Demo to show platform specific samples !
    /// 
    /// It will emit console outputs starting with "CI-TRACE:" which are checked
    /// in the test run - so DON'T REMOVE them.
    /// 
    /// </summary>
    internal static class Program
    {
        private const string TraceSignature = "CI-TRACE:";

        private static void CiTrace(string key, string value)
        {
            Console.WriteLine($"{TraceSignature} {key}={value}");
        }

        private static Stopwatch _startupTimer;

        private static int Main(string[] args)
        {
            CiTrace("Application", "Started");
            // measure startup time (maybe including CEF download)
            _startupTimer = new Stopwatch();
            _startupTimer.Start();

            var core = typeof(IChromatronConfiguration).Assembly;
            CiTrace("Chromatron.Core", core.GetName().Version?.ToString() ?? "");
            CiTrace("Platform", ChromatronRuntime.Platform.ToString());

            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            CiTrace("AppDirectory", appDirectory);
            var startUrl = $"file:///{appDirectory}/index.html";

            var config = DefaultConfiguration.CreateForRuntimePlatform();
            config.CefDownloadOptions = new CefDownloadOptions(true, true);
            config.WindowOptions.Position = new WindowPosition(1, 2);
            config.WindowOptions.Size = new WindowSize(1000, 600);
            config.StartUrl = startUrl;
            config.DebuggingMode = true;
            config.WindowOptions.RelativePathToIconFile = "chromatron.ico";

            CiTrace("Configuration", "Created");

            try
            {
                var builder = AppBuilder.Create(args);
                builder = builder.UseConfig<DefaultConfiguration>(config);
                builder = builder.UseWindow<TestWindow>();
                builder = builder.UseApp<ChromatronBasicApp>();
                builder = builder.Build();
                builder.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            CiTrace("Application", "Done");
            return 0;
        }
    }

    public class TestWindow : Window
    {
        public TestWindow(IChromatronNativeHost nativeHost,
                      IChromatronConfiguration config,
                      ChromatronHandlersResolver handlersResolver)
            : base(nativeHost, config, handlersResolver)
        {

            #region Events
            FrameLoadStart += TestWindow_FrameLoadStart;
            FrameLoadEnd += TestWindow_FrameLoadEnd;
            LoadingStateChanged += TestWindow_LoadingStateChanged;
            ConsoleMessage += TestWindow_ConsoleMessage;
            AddressChanged += TestWindow_AddressChanged;
            #endregion Events

        }

        #region Events
        private void TestWindow_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            Console.WriteLine("AddressChanged event called.");
        }

        private void TestWindow_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine("ConsoleMessage event called.");
        }

        private void TestWindow_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            Console.WriteLine("LoadingStateChanged event called.");
        }

        private void TestWindow_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            Console.WriteLine("FrameLoadStart event called.");
        }

        private void TestWindow_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Console.WriteLine("FrameLoadEnd event called.");
        }

        #endregion Events
    }
}

