// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Tests.ChromatronCore;

public class ChromatronRouteProviderTests
{
    private readonly IChromatronRouteProvider _routeProvider = Startup.GetProvider().GetRequiredService<IChromatronRouteProvider>();
    private readonly List<ChromatronController> _controllers = Startup.GetProvider().GetServices<ChromatronController>().ToList();

    [OneTimeSetUp]
    public void ChromatronRouteProviderTestsSetup()
    {
        _routeProvider.RegisterAllRoutes(_controllers);
    }

    [Test]
    public void EnsureAllRoutesAreRegisteredTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            var key = RouteKeys.CreateActionKey(routePathItem.Value);
            _routeProvider.RouteMap.ContainsKey(key).Should().BeTrue();
        }
    }

    [Test]
    public void RouteExistsTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            _routeProvider.RouteExists("http://chromatron.com" + routePathItem.Value).Should().BeTrue();
        }
    }

    [Test]
    public void GetRouteTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            _routeProvider.GetRoute("http://chromatron.com" + routePathItem.Value).Should().NotBeNull();
        }
    }

    [Test]
    public void IsRouteAsyncTest()
    {
        foreach (var routePathItem in TodoController.GetRoutePaths)
        {
            if (routePathItem.Value.Contains("/async/"))
            {
                _routeProvider.IsRouteAsync("http://chromatron.com" + routePathItem.Value).Should().BeTrue();
            }
            else
            {
                _routeProvider.IsRouteAsync("http://chromatron.com" + routePathItem.Value).Should().BeFalse();
            }
        }
    }
}