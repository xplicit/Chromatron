// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Configuration;

/// <summary>
/// Implements default values for <see cref="IChromatronConfiguration"/>.
/// </summary>
public class DefaultConfiguration : IChromatronConfiguration
{
    /// <summary>
    /// Initializes a new instance of <see cref="DefaultConfiguration"/>.
    /// </summary>
    public DefaultConfiguration()
    {
        AppName = Assembly.GetEntryAssembly()?.GetName().Name ?? "Chromatron App";
        Platform = ChromatronRuntime.Platform;
        AppExeLocation = AppDomain.CurrentDomain.BaseDirectory;
        StartUrl = "local://app/index.html";
        DebuggingMode = true;
        UrlSchemes = new List<UrlScheme>();
        CefDownloadOptions = new CefDownloadOptions();
        WindowOptions = new WindowOptions();
        if (string.IsNullOrWhiteSpace(WindowOptions.Title))
        {
            WindowOptions.Title = AppName;
        }

        // These are all default schemes.
        // They can be removed or replaced.
        UrlSchemes.AddRange(new List<UrlScheme>()
            {
                new UrlScheme(DefaultSchemeName.LOCALRESOURCE, "local", string.Empty, string.Empty, UrlSchemeType.LocalResource),
                new UrlScheme(DefaultSchemeName.LOCALREQUEST, "http", "chromatron.com", string.Empty, UrlSchemeType.LocalRequest),
                new UrlScheme(DefaultSchemeName.OWIN, "http", "chromatron.owin.com", string.Empty, UrlSchemeType.Owin),
                new UrlScheme(DefaultSchemeName.GITHUBSITE, string.Empty, string.Empty, "https://github.com/xplicit/Chromatron", UrlSchemeType.ExternalBrowser, true)
            });

        CustomSettings = new Dictionary<string, string>()
        {
            ["cefLogFile"] = Path.Combine("logs","chromatron.cef.log"),
            ["logSeverity"] = "info",
            ["locale"] = "en-US"
        };
    }

    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    public string? AppName { get; set; }

    /// <summary>
    /// Gets or sets the start URL.
    /// </summary>
    public string StartUrl { get; set; }

    /// <summary>
    /// Gets or sets the application executable location.
    /// </summary>
    public string AppExeLocation { get; set; }

    /// <summary>
    /// Gets or sets the Chromatron version.
    /// </summary>
    public string? ChromatronVersion { get; set; }

    /// <summary>
    /// Gets or sets the platform.
    /// </summary>
    public ChromatronPlatform Platform { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether debugging is enabled or not.
    /// </summary>
    public bool DebuggingMode { get; set; }

    /// <summary>
    /// Gets or sets the dev tools URL.
    /// </summary>
    public string? DevToolsUrl { get; set; }

    /// <summary>
    /// Gets or sets the command line arguments.
    /// </summary>
    public Dictionary<string, string>? CommandLineArgs { get; set; }

    /// <summary>
    /// Gets or sets the command line options.
    /// </summary>
    public List<string>? CommandLineOptions { get; set; }

    /// <summary>
    /// Gets or sets the custom settings.
    /// </summary>
    public Dictionary<string, string>? CustomSettings { get; set; }

    /// <summary>
    /// Gets or sets the extension data.
    /// </summary>
    public Dictionary<string, object>? ExtensionData { get; set; }

    /// <summary>
    /// Gets or sets the java script executor.
    /// </summary>
    public IChromatronJavaScriptExecutor? JavaScriptExecutor { get; set; }

    /// <summary>
    /// Gets or sets the URL schemes.
    /// </summary>
    public List<UrlScheme> UrlSchemes { get; set; }

    /// <summary>
    /// Gets or sets <see cref="Configuration.CefDownloadOptions"/> for CEF download options.
    /// </summary>
    public CefDownloadOptions CefDownloadOptions { get; set; }

    /// <summary>
    /// Gets or sets <see cref="IWindowOptions"/> for window options.
    /// </summary>
    public IWindowOptions WindowOptions { get; set; }

    /// <summary>
    /// Create configuration instance for the OS platform app is running on.
    /// </summary>
    /// <returns>Instance of <see cref="IChromatronConfiguration"/>.</returns>
    public static IChromatronConfiguration CreateForRuntimePlatform()
    {
        return CreateForPlatform(ChromatronRuntime.Platform);
    }

    /// <summary>
    /// Create configuration instance for OS platform specifying the platform.
    /// </summary>
    /// <param name="platform">Specifying the OS platfor to create <see cref="IChromatronConfiguration"/> instance for.</param>
    /// <returns>Instance of <see cref="IChromatronConfiguration"/>.</returns>
    public static IChromatronConfiguration CreateForPlatform(ChromatronPlatform platform)
    {
        IChromatronConfiguration config = new DefaultConfiguration();

        try
        {
            switch (platform)
            {
                case ChromatronPlatform.Windows:
                    config.WindowOptions.CustomStyle = new WindowCustomStyle(0, 0);
                    config.WindowOptions.UseCustomStyle = false;
                    break;

                case ChromatronPlatform.Linux:
                    config.CommandLineArgs = new Dictionary<string, string>
                    {
                        ["disable-gpu"] = "1"
                    };

                    config.CommandLineOptions = new List<string>()
                        {
                            "no-zygote",
                            "disable-gpu"
                        };
                    break;

                case ChromatronPlatform.MacOSX:
                    break;
            }

            return config;
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }

        return config;
    }
}