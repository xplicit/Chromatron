// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// The CefGlue resource scheme handler factory.
/// </summary>
public class DefaultResourceSchemeHandlerFactory : CefSchemeHandlerFactory
{
    protected readonly IChromatronConfiguration _config;
    protected readonly IChromatronErrorHandler _chromatronErrorHandler;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultResourceSchemeHandlerFactory"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    /// <param name="chromatronErrorHandler">Instance of <see cref="IChromatronErrorHandler"/>.</param>
    public DefaultResourceSchemeHandlerFactory(IChromatronConfiguration config, IChromatronErrorHandler chromatronErrorHandler)
    {
        _config = config;
        _chromatronErrorHandler = chromatronErrorHandler;
    }

    /// <inheritdoc/>
    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {
        return new DefaultResourceSchemeHandler(_config, _chromatronErrorHandler);
    }
}