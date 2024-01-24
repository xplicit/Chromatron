// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Defaults;

/// <summary>
/// The default implementation of <see cref="IChromatronRouteProvider"/>.
/// </summary>
public class DefaultRouteProvider : IChromatronRouteProvider
{
    protected readonly IChromatronModelBinder _routeParameterBinder;
    protected readonly IChromatronDataTransferOptions _dataTransferOptions;

    /// <summary>
    /// Initializes a new instance of <see cref="IChromatronRouteProvider"/>.
    /// </summary>
    /// <param name="routeParameterBinder">The <see cref="IChromatronModelBinder"/> instance.</param>
    /// <param name="dataTransferOptions">The <see cref="IChromatronDataTransferOptions"/> instance.</param>
    public DefaultRouteProvider(IChromatronModelBinder routeParameterBinder, IChromatronDataTransferOptions dataTransferOptions)
    {
        _routeParameterBinder = routeParameterBinder;
        _dataTransferOptions = dataTransferOptions;

        RouteMap = new Dictionary<string, ControllerRoute>();
    }

    /// <inheritdoc />
    public IDictionary<string, ControllerRoute> RouteMap { get; }

    /// <inheritdoc />
    public IList<string> RouteKeys
    {
        get
        {
            var tempData = RouteMap?.Keys;
            if (tempData is not null)
            {
                return tempData.ToList();
            }

            return new List<string>();
        }
    }

    /// <inheritdoc />
    public virtual void RegisterAllRoutes(List<ChromatronController>? controllers)
    {
        if (controllers is null || !controllers.Any())
        {
            return;
        }

        try
        {

            foreach (var controller in controllers)
            {
                var controllerRoutesFactory = new ControllerRoutesFactory();
                ControllerRoutesFactory.CreateAndRegisterRoutes(this, controller, _routeParameterBinder, _dataTransferOptions);
            }

        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }
    }

    /// <inheritdoc />
    public virtual void RegisterRoute(string key, ControllerRoute route)
    {
        RouteMap.Add(key, route);
    }

    /// <inheritdoc />
    public virtual void RegisterRoutes(IDictionary<string, ControllerRoute>? routeMap)
    {
        if (routeMap is not null && routeMap.Any())
        {
            foreach (var item in routeMap)
            {
                RegisterRoute(item.Key, item.Value);
            }
        }
    }

    /// <inheritdoc />
    public virtual ControllerRoute? GetRoute(string routeUrl)
    {
        var key = Network.RouteKeys.CreateActionKey(routeUrl);
        if (string.IsNullOrWhiteSpace(key))
        {
            return default;
        }

        if (RouteMap.ContainsKey(key))
        {
            return RouteMap[key];
        }

        return default;
    }

    /// <inheritdoc />
    public virtual bool RouteExists(string routeUrl)
    {
        var keys = RouteKeys;
        if (!keys.IsNullOrEmpty())
        {
            var key = Network.RouteKeys.CreateActionKey(routeUrl);
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return keys.Contains(key);
        }

        return false;
    }

    /// <inheritdoc />
    public virtual bool IsRouteAsync(string routeUrl)
    {
        try
        {
            var route = GetRoute(routeUrl);
            return route is not null && route.IsAsync;
        }
        catch { }

        return false;
    }
}