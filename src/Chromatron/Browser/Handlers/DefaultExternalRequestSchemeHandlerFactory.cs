// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// Handler factory object. Uses Shared HttpClient for all handlers
/// </summary>
public class DefaultExternalRequestSchemeHandlerFactory : CefSchemeHandlerFactory
{
    static readonly HttpClient _sharedClient = new HttpClient(
        new HttpClientHandler
        {
            AllowAutoRedirect = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        }, true);


    /// <summary>
    /// Initializes a new instance of the Chromatron.CefGlue.Browser.Handlers.ExternalRequestSchemeHandlerFactory class.
    /// </summary>
    public DefaultExternalRequestSchemeHandlerFactory()
    {

    }

    /// <inheritdoc/>
    protected override CefResourceHandler Create(CefBrowser browser, CefFrame frame, string schemeName, CefRequest request)
    {
        return new DefaultExternalRequestSchemeHandler(_sharedClient);
    }
}