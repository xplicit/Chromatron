// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// Default CEF http scheme handler factory.
/// </summary>
public class DefaultRequestSchemeHandlerFactory : CefSchemeHandlerFactory
{
    protected readonly IChromatronRouteProvider _routeProvider;
    protected readonly IChromatronRequestSchemeProvider _requestSchemeProvider;
    protected readonly IChromatronRequestHandler _requestHandler;
    protected readonly IChromatronDataTransferOptions _dataTransferOptions;
    protected readonly IChromatronErrorHandler _chromatronErrorHandler;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultRequestSchemeHandlerFactory"/>.
    /// </summary>
    /// <param name="routeProvider">Instance of <see cref="IChromatronRouteProvider"/>.</param>
    /// <param name="requestSchemeProvider">Instance of <see cref="IChromatronRequestSchemeProvider"/>.</param>
    /// <param name="requestHandler">Instance of <see cref="IChromatronRequestHandler"/>.</param>
    /// <param name="dataTransferOptions">Instance of <see cref="IChromatronDataTransferOptions"/>.</param>
    /// <param name="chromatronErrorHandler">Instance of <see cref="IChromatronErrorHandler"/>.</param>
    public DefaultRequestSchemeHandlerFactory(IChromatronRouteProvider routeProvider,
                                              IChromatronRequestSchemeProvider requestSchemeProvider,
                                              IChromatronRequestHandler requestHandler,
                                              IChromatronDataTransferOptions dataTransferOptions,
                                              IChromatronErrorHandler chromatronErrorHandler)
    {
        _routeProvider = routeProvider;
        _requestSchemeProvider = requestSchemeProvider;
        _requestHandler = requestHandler;
        _dataTransferOptions = dataTransferOptions;
        _chromatronErrorHandler = chromatronErrorHandler;
    }

    /// <inheritdoc/>
    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {
        return new DefaultRequestSchemeHandler(_routeProvider, _requestSchemeProvider, _requestHandler, _dataTransferOptions, _chromatronErrorHandler);
    }
}