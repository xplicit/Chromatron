// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Owin;

/// <summary>
/// The default implementation of <see cref="ChromatronOwinApp"/> class.
/// </summary>
public abstract class ChromatronOwinApp : ChromatronAppBase, IOwinAppStartup
{
    /// <summary>
    /// Initializes a new instance of <see cref="ChromatronOwinApp"/>
    /// </summary>
    public ChromatronOwinApp()
    {
        Environment = "Development";
        ErrorHandlingPath = "/Home/Error";
    }

    /// <inheritdoc />
    public string? Environment { get; set; }

    /// <inheritdoc />
    public string? ErrorHandlingPath { get; set; }

    /// <inheritdoc />
    public IConfiguration? Configuration { get; set; }

    /// <inheritdoc />
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
    }

    /// <inheritdoc />
    public sealed override void ConfigureCoreServices(IServiceCollection services)
    {
        // Ensure that the base method is called last
        base.ConfigureCoreServices(services);

        services.TryAddSingleton<IOwinPipeline, OwinPipeline>();
    }
    /// <inheritdoc />
    public sealed override void ConfigureDefaultHandlers(IServiceCollection services)
    {
        base.ConfigureDefaultHandlers(services);

        services.AddSingleton<IChromatronErrorHandler, ChromatronOwinErrorHandler>();
        services.AddSingleton<CefSchemeHandlerFactory, DefaultOwinSchemeHandlerFactory>();
    }

    /// <inheritdoc />
    public abstract void Configure(IConfigurationBuilder configBuilder);

    /// <inheritdoc />
    public abstract void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory);

    /// <inheritdoc />
    public abstract void Configure(IWebHostBuilder builder);

    /// <inheritdoc />
    public abstract void Configure(IWebHost host);
}