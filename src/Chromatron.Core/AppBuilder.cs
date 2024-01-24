// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core;

/// <inheritdoc/>
public sealed class AppBuilder : AppBuilderBase
{
    private ChromatronServiceProviderFactory? _serviceProviderFactory;

    /// <inheritdoc/>
    private AppBuilder(string[] args)
        : base(args)
    {
    }

    /// <summary>
    /// Creates the application builder instance.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public static AppBuilderBase Create(string[] args)
    {
        var appBuilder = new AppBuilder(args);
        return appBuilder;
    }

    /// <summary>
    /// Allows the developer to use an external <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>
    /// Usually custom <see cref="IServiceCollection"/> is not needed but one can be provided.
    /// </remarks>
    /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public AppBuilderBase UseServices(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        return this;
    }

    /// <summary>
    /// Allows the developer to use an external <see cref="IServiceProvider"/> creator.
    /// </summary>
    /// <param name="serviceProviderFactory">A custom The <see cref="ChromatronServiceProviderFactory" /> instance.</param>
    /// <returns>Instance of the <see cref="AppBuilder"/>.</returns>
    public AppBuilderBase UseServiceProviderFactory(ChromatronServiceProviderFactory serviceProviderFactory)
    {
        _serviceProviderFactory = serviceProviderFactory;
        return this;
    }

    /// <inheritdoc/>
    public override AppBuilderBase Build()
    {
        if (_stepCompleted != 1)
        {
            throw new Exception("Invalid order: Step 1: UseApp must be completed before Step 2: Build.");
        }

        if (_chromatronApp is null)
        {
            throw new Exception($"ChromatronApp {nameof(_chromatronApp)} cannot be null.");
        }

        if (_serviceCollection is null)
        {
            _serviceCollection = new ServiceCollection();
        }

        _chromatronApp.ConfigureServices(_serviceCollection);

        // This must be done before registering core services
        RegisterUseComponents(_serviceCollection);

        _chromatronApp.ConfigureCoreServices(_serviceCollection);
        _chromatronApp.ConfigureServicesResolver(_serviceCollection);
        _chromatronApp.ConfigureDefaultHandlers(_serviceCollection);

        _serviceProvider = _serviceProviderFactory is not null
            ? _serviceProviderFactory.BuildServiceProvider(_serviceCollection)
            : _serviceCollection.BuildServiceProvider();

        _chromatronApp.Initialize(_serviceProvider);
        _chromatronApp.RegisterChromatronControllerRoutes(_serviceProvider);

        _stepCompleted = 2;
        return this;
    }

    /// <inheritdoc/>
    public override void Run()
    {
        if (_stepCompleted != 2)
        {
            throw new Exception("Invalid order: Step 2: Build must be completed before Step 3: Run.");
        }

        if (_serviceProvider is null)
        {
            throw new Exception("ServiceProvider is not initialized.");
        }

        try
        {
            var appName = Assembly.GetEntryAssembly()?.GetName().Name;
            var windowController = _serviceProvider.GetService<ChromatronWindowController>();
            if (windowController is null)
            {
                throw new Exception("ChromatronWindowController is not registered.");
            }

            try
            {
                Logger.Instance.Log.LogInformation("Running application:{appName}.", appName);
                windowController.Run(_args);
            }
            catch (Exception exception)
            {
                Logger.Instance.Log.LogError(exception, "Error running application:{appName}.", appName);
            }
            finally
            {
                windowController.Dispose();
                (_serviceProvider as ServiceProvider)?.Dispose();
            }

        }
        catch (Exception exception)
        {
            var appName = Assembly.GetEntryAssembly()?.GetName().Name;
            Logger.Instance.Log.LogError(exception, "Error running application:{appName}.", appName);
        }
    }
}