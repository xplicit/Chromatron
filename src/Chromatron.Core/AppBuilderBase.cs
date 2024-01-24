// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core;

/// <summary>
/// Base application builder class.
/// </summary>
public abstract class AppBuilderBase
{
    protected string[] _args;
    protected IServiceCollection? _serviceCollection;
    protected IServiceProvider? _serviceProvider;
    protected ChromatronApp? _chromatronApp;
    protected IChromatronConfiguration? _config;
    protected IChromatronWindow? _chromatronWindow;
    protected IChromatronErrorHandler? _chromatronErrorHandler;
    protected Type? _chromatronUseConfigType;
    protected Type? _chromatronUseWindowType;
    protected Type? _chromatronUseErrorHandlerType;
    protected int _stepCompleted;

    /// <summary>
    /// Initializes a new instance of <see cref="AppBuilderBase"/>.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    protected AppBuilderBase(string[] args)
    {
        _args = args;
        _config = null;
        _chromatronUseConfigType = null;
        _chromatronUseWindowType = null;
        _chromatronUseErrorHandlerType = null;
        _stepCompleted = -1;
    }

    /// <summary>
    /// Allows the developer to use custom configuration of derived type of <see cref="IChromatronConfiguration"/> 
    /// or an instance of derived type of <see cref="IChromatronConfiguration"/>.
    /// </summary>
    /// <remarks>
    /// If an instance of <see cref="IChromatronConfiguration" /> is provided as a parameter, that is what is used.
    /// If the instance of <see cref="IChromatronConfiguration" /> is not provided as a parameter, the type of TServive is used to create one.
    /// </remarks>
    /// <typeparam name="TService">A derived type of <see cref="IChromatronConfiguration" /> definition.</typeparam>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public virtual AppBuilderBase UseConfig<TService>(IChromatronConfiguration? config = null) where TService : IChromatronConfiguration
    {
        if (config is not null)
        {
            _config = config;
        }
        else
        {
            _chromatronUseConfigType = null;
            typeof(TService).EnsureIsDerivedFromType<IChromatronConfiguration>();
            _chromatronUseConfigType = typeof(TService);
        }

        return this;
    }

    /// <summary>
    /// Allows the developer to use custom window of derived type of <see cref="IChromatronWindow"/> 
    /// or an instance of derived type of <see cref="IChromatronWindow"/>.
    /// </summary>
    /// <remarks>
    /// If an instance of <see cref="IChromatronWindow" /> is provided as a parameter, that is what is used.
    /// If the instance of <see cref="IChromatronWindow" /> is not provided as a parameter, the type of TServive is used to create one.
    /// </remarks>
    /// <typeparam name="TService">Type of <see cref="IChromatronWindow"/>.</typeparam>
    /// <param name="chromatronWindow">The <see cref="IChromatronWindow" /> instance.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public virtual AppBuilderBase UseWindow<TService>(IChromatronWindow? chromatronWindow = null) where TService : IChromatronWindow
    {
        if (chromatronWindow is not null)
        {
            _chromatronWindow = chromatronWindow;
        }
        else
        {
            _chromatronUseWindowType = null;
            typeof(TService).EnsureIsDerivedFromType<IChromatronWindow>();
            _chromatronUseWindowType = typeof(TService);
        }

        return this;
    }

    /// <summary>
    /// Allows the developer to use custom error handler of derived type of <see cref="IChromatronErrorHandler"/> 
    /// or an instance of derived type of <see cref="IChromatronErrorHandler"/>.
    /// </summary>
    /// <remarks>
    /// If an instance of <see cref="IChromatronErrorHandler" /> is provided as a parameter, that is what is used.
    /// If the instance of <see cref="IChromatronErrorHandler" /> is not provided as a parameter, the type of TServive is used to create one.
    /// </remarks>
    /// <typeparam name="TService">Type of <see cref="IChromatronErrorHandler"/>.</typeparam>
    /// <param name="chromatronErrorHandler">The <see cref="IChromatronErrorHandler" /> instance.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public virtual AppBuilderBase UseErrorHandler<TService>(IChromatronErrorHandler? chromatronErrorHandler = null) where TService : IChromatronErrorHandler
    {
        if (chromatronErrorHandler is not null)
        {
            _chromatronErrorHandler = chromatronErrorHandler;
        }
        else
        {
            _chromatronUseErrorHandlerType = null;
            typeof(TService).EnsureIsDerivedFromType<IChromatronErrorHandler>();
            _chromatronUseErrorHandlerType = typeof(TService);
        }

        return this;
    }

    /// <summary>
    /// Allows the developer to use custom application class of derived type of <see cref="ChromatronApp"/> 
    /// or an instance of derived type of <see cref="ChromatronApp"/>.
    /// </summary>
    /// <remarks>
    /// If an instance of <see cref="ChromatronApp" /> is provided as a parameter, that is what is used.
    /// If the instance of <see cref="ChromatronApp" /> is not provided as a parameter, the type of TApp is used to create one.
    /// </remarks>
    /// <typeparam name="TApp">Type of <see cref="IChromatronErrorHandler"/>.</typeparam>
    /// <param name="chromatronApp">The <see cref="ChromatronApp" /> instance.</param>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public virtual AppBuilderBase UseApp<TApp>(ChromatronApp? chromatronApp = null) where TApp : ChromatronApp
    {
        _chromatronApp = chromatronApp;
        if (_chromatronApp is null)
        {
            typeof(TApp).EnsureIsDerivedFromType<ChromatronApp>();
            _chromatronApp = Activator.CreateInstance(typeof(TApp)) as TApp;
        }

        _stepCompleted = 1;
        return this;
    }

    /// <summary>
    /// Builds the application based on configured default and custom conifigured services.
    /// </summary>
    /// <returns>the instance of <see cref="AppBuilderBase"/>.</returns>
    public abstract AppBuilderBase Build();

    /// <summary>
    /// Runs the application.
    /// </summary>
    public abstract void Run();

    /// <summary>
    /// Register all the custom configured services.
    /// </summary>
    /// <param name="services"></param>
    protected void RegisterUseComponents(IServiceCollection services)
    {
        #region IChromatronConfiguration

        if (_config is not null)
        {
            services.TryAddSingleton<IChromatronConfiguration>(_config);
        }
        else if (_chromatronUseConfigType is not null)
        {
            services.TryAddSingleton(typeof(IChromatronConfiguration), _chromatronUseConfigType);
        }

        #endregion IChromatronConfiguration

        #region IChromatronWindow

        if (_chromatronWindow is not null)
        {
            services.TryAddSingleton<IChromatronWindow>(_chromatronWindow);
        }
        else if (_chromatronUseWindowType is not null)
        {
            services.TryAddSingleton(typeof(IChromatronWindow), _chromatronUseWindowType);
        }

        #endregion IChromatronWindow

        #region IChromatronErrorHandler

        if (_chromatronErrorHandler is not null)
        {
            services.TryAddSingleton<IChromatronErrorHandler>(_chromatronErrorHandler);
        }
        else if (_chromatronUseErrorHandlerType is not null)
        {
            services.TryAddSingleton(typeof(IChromatronErrorHandler), _chromatronUseErrorHandlerType);
        }

        #endregion IChromatronErrorHandler
    }
}