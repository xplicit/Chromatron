// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron;

/// <inheritdoc/>
public partial class WindowController : ChromatronWindowController
{
    protected readonly IChromatronRequestSchemeProvider _requestSchemeProvider;
    protected readonly ICefDownloader _binariesDownloader;

    /// <inheritdoc/>
    public WindowController(IChromatronWindow window,
                            IChromatronNativeHost nativeHost,
                            IChromatronConfiguration config,
                            IChromatronRouteProvider routeProvider,
                            IChromatronRequestHandler requestHandler,
                            IChromatronRequestSchemeProvider requestSchemeProvider,
                            ICefDownloader binariesDownloader,
                            ChromatronHandlersResolver handlersResolver)
        : base(window, nativeHost, config, routeProvider, requestHandler, handlersResolver)
    {
        // WindowController.NativeWindow
        _nativeHost.HostCreated += OnWindowCreated;
        _nativeHost.HostMoving += OnWindowMoving;
        _nativeHost.HostSizeChanged += OnWindowSizeChanged;
        _nativeHost.HostClose += OnWindowClose;

        _requestSchemeProvider = requestSchemeProvider;
        _binariesDownloader = binariesDownloader;
    }
}