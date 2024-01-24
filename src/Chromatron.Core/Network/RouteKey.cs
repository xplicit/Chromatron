// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Network;

public static class RouteKey
{
    public static string CreateRequestKey(string? url)
    {
        url = url?.Trim().TrimStart('/');
        var routeKey = $"action_{url}".Replace("/", "_").Replace("\\", "_");
        return routeKey.ToLowerInvariant();
    }

    public static string CreateCommandKey(string? url)
    {
        url = url?.Trim().TrimStart('/');
        var routeKey = $"commmand_{url}".Replace("/", "_").Replace("\\", "_");
        return routeKey.ToLowerInvariant();
    }
}