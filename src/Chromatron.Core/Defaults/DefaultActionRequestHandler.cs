// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Defaults;

/// <summary>
/// The default implementation of <see cref="IChromatronRequestHandler"/>.
/// </summary>
public class DefaultActionRequestHandler : IChromatronRequestHandler
{
    protected readonly IChromatronRouteProvider _routeProvider;
    protected readonly IChromatronInfo _chromatronInfo;
    protected readonly IChromatronErrorHandler _chromatronErrorHandler;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultActionRequestHandler"/>.
    /// </summary>
    /// <param name="routeProvider">The router provider - instance of <see cref="IChromatronRouteProvider"/>.</param>
    /// <param name="chromatronInfo">The chromatron info - instance of <see cref="IChromatronInfo"/>.</param>
    /// <param name="chromatronErrorHandler">Main Chromatron error handler - instance of <see cref="IChromatronErrorHandler"/>.</param>
    public DefaultActionRequestHandler(IChromatronRouteProvider routeProvider, IChromatronInfo chromatronInfo, IChromatronErrorHandler chromatronErrorHandler)
    {
        _routeProvider = routeProvider;
        _chromatronInfo = chromatronInfo;
        _chromatronErrorHandler = chromatronErrorHandler;
    }

    /// <inheritdoc/>
    public void Execute(string url)
    {
        var routePath = url.GetPathFromUrl();
        var parameters = url.GetParameters();

        ExecuteRoute(string.Empty, routePath, parameters, null, string.Empty);
    }

    /// <inheritdoc/>
    public IChromatronResponse Execute(string requestId, string routePath, IDictionary<string, object>? parameters, object? postData, string? requestData)
    {
        if (string.IsNullOrWhiteSpace(routePath))
        {
            return _chromatronErrorHandler.HandleRouteNotFound(requestId, routePath);
        }

        if (routePath.Equals("/info", StringComparison.OrdinalIgnoreCase))
        {
            return _chromatronInfo.GetInfo(requestId);
        }

        var route = _routeProvider.GetRoute(routePath);
        if (route is null)
        {
            throw new Exception($"Route for path = {routePath} is null or invalid.");
        }

        return ExecuteRoute(requestId, routePath, parameters, postData, requestData);
    }

    /// <inheritdoc/>
    public async Task<IChromatronResponse> ExecuteAsync(string requestId, string routePath, IDictionary<string, object>? parameters, object? postData, string? requestData)
    {
        if (string.IsNullOrWhiteSpace(routePath))
        {
            return _chromatronErrorHandler.HandleRouteNotFound(requestId, routePath);
        }

        if (routePath.Equals("/info", StringComparison.OrdinalIgnoreCase))
        {
            return _chromatronInfo.GetInfo(requestId);
        }

        var route = _routeProvider.GetRoute(routePath);
        if (route is null)
        {
            return _chromatronErrorHandler.HandleRouteNotFound(requestId, routePath);
        }

        return await ExecuteRouteAsync(requestId, routePath, parameters, postData, requestData);
    }

    private IChromatronResponse ExecuteRoute(string requestId, string routeUrl, IDictionary<string, object>? parameters, object? postData, string? requestData)
    {
        var route = _routeProvider.GetRoute(routeUrl);

        if (route is null)
        {
            return _chromatronErrorHandler.HandleRouteNotFound(requestId, routeUrl);
        }

        var request = new ChromatronRequest(requestId, routeUrl, parameters, postData, requestData);
        var response = route.Invoke(request);
        response.ReadyState = (int)ReadyState.ResponseIsReady;
        response.Status = (response.Status == 0) ? (int)HttpStatusCode.OK : response.Status;
        response.StatusText = (string.IsNullOrWhiteSpace(response.StatusText) && (response.Status == (int)HttpStatusCode.OK)) ? "OK" : response.StatusText;

        return response;
    }

    private async Task<IChromatronResponse> ExecuteRouteAsync(string requestId, string routeUrl, IDictionary<string, object>? parameters, object? postData, string? requestData)
    {
        var route = _routeProvider.GetRoute(routeUrl);

        if (route is null)
        {
            return _chromatronErrorHandler.HandleRouteNotFound(requestId, routeUrl);
        }

        IChromatronResponse response;
        var request = new ChromatronRequest(requestId, routeUrl, parameters, postData, requestData);


        response = await route.InvokeAsync(request);

        response.ReadyState = (int)ReadyState.ResponseIsReady;
        response.Status = (response.Status == 0) ? (int)HttpStatusCode.OK : response.Status;
        response.StatusText = (string.IsNullOrWhiteSpace(response.StatusText) && (response.Status == (int)HttpStatusCode.OK)) ? "OK" : response.StatusText;

        return response;
    }
}