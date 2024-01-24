// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Tests.ChromatronCore;

public class ChromatronRouteProviderTests
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IChromatronRouteProvider _routeProvider;
    private readonly List<ChromatronController> _controllers;

    public ChromatronRouteProviderTests(IServiceProvider serviceProvider, IChromatronRouteProvider routeProvider)
    {
        _serviceProvider = serviceProvider;
        _routeProvider = routeProvider;
        _controllers = _serviceProvider.GetServices<ChromatronController>().ToList();
        routeProvider.RegisterAllRoutes(_controllers);
    }

    [Fact]
    public void EnsureAllRoutesAreRegisteredTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            var key = RouteKeys.CreateActionKey(routePathItem.Value);
            Assert.True(_routeProvider.RouteMap.ContainsKey(key));
        }
    }

    [Fact]
    public void RouteExistsTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            Assert.True(_routeProvider.RouteExists("http://chromatron.com" + routePathItem.Value));
        }
    }

    [Fact]
    public void GetRouteTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            Assert.NotNull(_routeProvider.GetRoute("http://chromatron.com" + routePathItem.Value));
        }
    }

    [Fact]
    public void IsRouteAsyncTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            if (routePathItem.Value.Contains("/async/"))
            {
                Assert.True(_routeProvider.IsRouteAsync("http://chromatron.com" + routePathItem.Value));
            }
            else
            {
                Assert.False(_routeProvider.IsRouteAsync("http://chromatron.com" + routePathItem.Value));
            }
        }
    }
}