// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#pragma warning disable IDE1006

namespace Chromatron.Browser;

/// <summary>
/// Default CEF message router handler.
/// </summary>
/// <remarks>
/// Implements - https://bitbucket.org/chromiumembedded/cef/wiki/GeneralUsage.md#markdown-header-generic-message-router
/// </remarks>
public class DefaultMessageRouterHandler : CefMessageRouterBrowserSide.Handler, IChromatronMessageRouter
{
    protected readonly IChromatronRouteProvider _routeProvider;
    protected readonly IChromatronRequestHandler _requestHandler;
    protected readonly IChromatronDataTransferOptions _dataTransferOptions;
    protected readonly IChromatronErrorHandler _chromatronErrorHandler;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultMessageRouterHandler"/>.
    /// </summary>
    /// <param name="routeProvider">Instance of <see cref="IChromatronRouteProvider"/>.</param>
    /// <param name="requestHandler">Instance of <see cref="IChromatronRequestHandler"/>.</param>
    /// <param name="dataTransferOptions">Instance of <see cref="IChromatronDataTransferOptions"/>.</param>
    /// <param name="chromatronErrorHandler">Instance of <see cref="IChromatronErrorHandler"/>.</param>
    public DefaultMessageRouterHandler(IChromatronRouteProvider routeProvider, IChromatronRequestHandler requestHandler, IChromatronDataTransferOptions dataTransferOptions, IChromatronErrorHandler chromatronErrorHandler)
    {
        _routeProvider = routeProvider;
        _requestHandler = requestHandler;
        _dataTransferOptions = dataTransferOptions;
        _chromatronErrorHandler = chromatronErrorHandler;
    }

    /// <inheritdoc/>
    public override bool OnQuery(CefBrowser browser, CefFrame frame, long queryId, string request, bool persistent, CefMessageRouterBrowserSide.Callback callback)
    {
        request? requestData = null;

        try
        {
            requestData = JsonSerializer.Deserialize<request>(request, _dataTransferOptions.SerializerOptions as JsonSerializerOptions);

            if (requestData is not null)
            {
                var id = requestData.id ?? string.Empty;
                var path = requestData.url ?? string.Empty;

                bool isRequestAsync = _routeProvider.IsRouteAsync(path);

                if (isRequestAsync)
                {
                    Task.Run(async () =>
                    {
                        var parameters = requestData.parameters;
                        var postData = requestData.postData;

                        var response = await _requestHandler.ExecuteAsync(id, path, parameters, postData, request);
                        var jsonResponse = _dataTransferOptions.ConvertObjectToJson(response);

                        callback.Success(jsonResponse);
                    });
                }
                else
                {
                    Task.Run(() =>
                    {
                        var parameters = requestData.parameters;
                        var postData = requestData.postData;

                        var response = _requestHandler.Execute(id, path, parameters, postData, request);
                        var jsonResponse = _dataTransferOptions.ConvertObjectToJson(response);

                        callback.Success(jsonResponse);
                    });
                }

                return true;
            }
        }
        catch (Exception exception)
        {
            var chromatronRequest = requestData?.ToRequest();
            if (chromatronRequest is null)
            {
                chromatronRequest = new ChromatronRequest();
            }

            var response = _chromatronErrorHandler.HandleError(chromatronRequest, exception);
            var jsonResponse = _dataTransferOptions.ConvertObjectToJson(response);
            callback.Failure(100, jsonResponse);
            return false;
        }

        callback.Failure(100, "Request is not valid.");
        return false;
    }

    /// <inheritdoc/>
    public override void OnQueryCanceled(CefBrowser browser, CefFrame frame, long queryId)
    {
    }

    private class request
    {
        public request()
        {
            id = Guid.NewGuid().ToString();
            url = string.Empty;
        }

        public string id { get; set; }
        public string url { get; set; }
        public IDictionary<string, object>? parameters { get; set; }
        public object? postData { get; set; }

        public IChromatronRequest ToRequest()
        {
            return new ChromatronRequest(id, url, parameters, postData, null);
        }
    }
}
