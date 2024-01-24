// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// Default implementation of <see cref="CefRequestHandler"/>.
/// </summary>
public class DefaultRequestHandler : CefRequestHandler
{
    protected readonly IChromatronConfiguration _config;
    protected readonly IChromatronRequestHandler _requestHandler;
    protected readonly IChromatronRouteProvider _routeProvider;
    protected readonly CefResourceRequestHandler? _resourceRequestHandler;

    /// <summary>
    /// The m_browser.
    /// </summary>
    protected ChromiumBrowser? _browser;


    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultRequestHandler"/> class.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    /// <param name="requestHandler">Instance of <see cref="IChromatronRequestHandler"/>.</param>
    /// <param name="routeProvider">Instance of <see cref="IChromatronRouteProvider"/>.</param>
    /// <param name="window">Instance of <see cref="IChromatronWindow"/>.</param>
    /// <param name="resourceRequestHandler">Instance of <see cref="CefResourceRequestHandler"/>.</param>>
    public DefaultRequestHandler(IChromatronConfiguration config,
                                 IChromatronRequestHandler requestHandler,
                                 IChromatronRouteProvider routeProvider,
                                 IChromatronWindow window,
                                 CefResourceRequestHandler? resourceRequestHandler = null)
    {
        _config = config;
        _requestHandler = requestHandler;
        _routeProvider = routeProvider;
        _browser = window as ChromiumBrowser;
        _resourceRequestHandler = resourceRequestHandler;
    }

    /// <summary>
    /// Gets or sets the browser.
    /// </summary>
    public ChromiumBrowser? Browser
    {
        get { return _browser; }
        set { _browser = value; }
    }

    /// <inheritdoc/>
    protected override CefResourceRequestHandler GetResourceRequestHandler(CefBrowser browser, CefFrame frame, CefRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return _resourceRequestHandler;
#pragma warning restore CS8603 // Possible null reference return.
    }

    /// <inheritdoc/>
    protected override bool OnBeforeBrowse(CefBrowser browser, CefFrame frame, CefRequest request, bool userGesture, bool isRedirect)
    {
        if (_config is not null)
        {
            var isUrlExternal = _config.UrlSchemes?.IsUrlRegisteredExternalBrowserScheme(request.Url);
            if (isUrlExternal.HasValue && isUrlExternal.Value)
            {
                BrowserLauncher.Open(_config.Platform, request.Url);
                return true;
            }
        }

        // Sample: http://chromatron.com/democontroller/showdevtools 
        // Expected to execute controller route action without return value
        var route = _routeProvider.GetRoute(request.Url);
        if (route is not null && !route.HasReturnValue)
        {
            _requestHandler.Execute(request.Url);
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    protected override void OnRenderProcessTerminated(CefBrowser browser, CefTerminationStatus status)
    {
        if (_browser is not null)
        {
            _browser.InvokeAsyncIfPossible(() => _browser.OnRenderProcessTerminated(new RenderProcessTerminatedEventArgs(status)));
        }
    }
}