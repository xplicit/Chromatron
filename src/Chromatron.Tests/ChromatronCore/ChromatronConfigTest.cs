// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Tests.ChromatronCore;

public class ChromatronConfigTests
{
    [Fact]
    public void ConfigTest()
    {
        // Arrange
        var appName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name;
        var moduleName = appName?.ToLower() ?? "";
        var inUnitTest = moduleName.Contains("testrunner") || moduleName.Contains("testhost");

        var windowTitle = inUnitTest ? DefaultConfig.WindowOptions.Title : appName;
        var platform = ChromatronRuntime.Platform;
        var appExeLocation = AppDomain.CurrentDomain.BaseDirectory;

        // Act
        var config = DefaultConfig;

        // Assert
        Assert.NotNull(config);
        Assert.Equal(appName, config.AppName);
        Assert.Equal(windowTitle, config.WindowOptions.Title);
        Assert.Equal(platform, config.Platform);
        Assert.Equal(appExeLocation, config.AppExeLocation);
    }

    private IChromatronConfiguration DefaultConfig
    {
        get
        {
            var config = new DefaultConfiguration();
            return config;
        }
    }

    private IChromatronConfiguration DefaultConfigFromFileExpectedValues
    {
        get
        {
            var config = new DefaultConfiguration
            {
                AppName = "chromatron_test",
                StartUrl = "local://app/chromatron.html",
                DebuggingMode = true,
                CefDownloadOptions = new CefDownloadOptions()
                {
                    AutoDownloadWhenMissing = true,
                    DownloadSilently = false
                },

                WindowOptions = new WindowOptions
                {
                    Size = new WindowSize(1200, 900),
                    Position = new WindowPosition(1, 2),
                    DisableResizing = false,
                    DisableMinMaximizeControls = false,
                    WindowFrameless = false,
                    StartCentered = true,
                    KioskMode = false,
                    WindowState = WindowState.Normal,
                    Title = "chromatron",
                    RelativePathToIconFile = "chromatron.ico",
                    CustomStyle = new WindowCustomStyle(0, 0),
                    UseCustomStyle = false
                },

                UrlSchemes = new List<UrlScheme>()
            };

            var schemeDefaultResource = new UrlScheme("default-local-resource", "local", string.Empty, string.Empty, UrlSchemeType.LocalResource, false);
            var schemeCustomHttp = new UrlScheme("default-request-http", "http", "chromatron.com", string.Empty, UrlSchemeType.LocalRequest, false);
            var schemeExternal1 = new UrlScheme("chromatron-site", string.Empty, string.Empty, "https://github.com/xplicit/Chromatron", UrlSchemeType.ExternalBrowser, true);

            config.UrlSchemes.Add(schemeDefaultResource);
            config.UrlSchemes.Add(schemeCustomHttp);
            config.UrlSchemes.Add(schemeExternal1);

            config.CustomSettings = new Dictionary<string, string>();
            config.CustomSettings["cefLogFile"] = "logs\\chromatron.cef.log";
            config.CustomSettings["logSeverity"] = "info";
            config.CustomSettings["locale"] = "en-US";

            config.CommandLineArgs = new Dictionary<string, string>();
            config.CommandLineArgs["disable-gpu"] = "1";

            config.CommandLineOptions = new List<string>();
            config.CommandLineOptions.Add("no-zygote");
            config.CommandLineOptions.Add("disable-gpu");
            config.CommandLineOptions.Add("disable-software-rasterizer");

            return config;
        }
    }
}