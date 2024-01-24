// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core;

/// <summary>
/// Represents the window controller class.
/// </summary>
/// <remarks>
/// The class "Run" method launches the application.
/// </remarks>
public abstract class ChromatronWindowController : IDisposable
{
    protected IChromatronWindow _window;
    protected IChromatronNativeHost _nativeHost;
    protected IChromatronRouteProvider _routeProvider;
    protected IChromatronConfiguration _config;
    protected IChromatronRequestHandler _requestHandler;
    protected ChromatronHandlersResolver _handlersResolver;

    /// <summary>
    /// Initializes a new instance of <see cref="ChromatronWindowController"/>.
    /// </summary>
    /// <param name="window">The main host window of type <see cref="IChromatronWindow" />.</param>
    /// <param name="nativeHost">The native host [Windows - win32, Linux - Gtk, MacOS - Cocoa] of type <see cref="IChromatronNativeHost"/>.</param>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    /// <param name="routeProvider">Instance of <see cref="IChromatronRouteProvider"/>.</param>
    /// <param name="requestHandler">Instance of <see cref="IChromatronRequestHandler"/>.</param>
    /// <param name="handlersResolver">Instance of <see cref="ChromatronHandlersResolver"/>.</param>
    public ChromatronWindowController(IChromatronWindow window,
                                    IChromatronNativeHost nativeHost,
                                    IChromatronConfiguration config,
                                    IChromatronRouteProvider routeProvider,
                                    IChromatronRequestHandler requestHandler,
                                    ChromatronHandlersResolver handlersResolver)
    {
        _window = window;
        _nativeHost = nativeHost;
        _config = config;
        _routeProvider = routeProvider;
        _requestHandler = requestHandler;
        _handlersResolver = handlersResolver;
    }

    /// <summary>
    /// Gets the host window - instance of <see cref="IChromatronWindow"/>.
    /// </summary>
    public IChromatronWindow Window => _window;

    /// <summary>
    /// Gets the native host - instance of <see cref="IChromatronNativeHost"/>.
    /// </summary>
    public IChromatronNativeHost NativeHost => _nativeHost;

    /// <summary>
    /// Gets the route provider - instance of <see cref="IChromatronRouteProvider"/>.
    /// </summary>
    public IChromatronRouteProvider RouteProvider => _routeProvider;

    /// <summary>
    /// Gets the configuration - instance of <see cref="IChromatronConfiguration"/>.
    /// </summary>
    public IChromatronConfiguration Config => _config;

    /// <summary>
    /// Gets the request handler - instance of <see cref="IChromatronRequestHandler"/>.
    /// </summary>
    public IChromatronRequestHandler RequestHandler => _requestHandler;

    #region Destructor

    /// <summary>
    /// Finalizes an instance of the <see cref="ChromatronWindowController"/> class. 
    /// </summary>
    ~ChromatronWindowController()
    {
        Dispose(false);
    }

    #endregion Destructor

    /// <summary>
    /// Runs the application.
    /// This call does not return until the application terminates
    /// or an error is occurred.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    /// <returns>
    ///  0 successfully run application - now terminated
    ///  1 on internal exception (see log for more information).
    /// </returns>
    public abstract int Run(string[] args);

    /// <summary>
    /// Exit the application.
    /// </summary>
    public abstract void Quit();

    #region Disposal

    private bool _disposed = false;

    /// <inheritdoc/>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        // If there are managed resources
        if (disposing)
        {
        }

        _nativeHost?.Dispose();
        _window?.Dispose();

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}