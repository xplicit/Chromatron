// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Network;

[AttributeUsage(AttributeTargets.Method)]
public class ChromatronRouteAttribute : Attribute
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? Description { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
public class ChromatronControllerAttribute : Attribute
{
    public ChromatronControllerAttribute()
    {
        RoutePath = string.Empty;
    }

    public string? Name { get; set; }
    public string RoutePath { get; set; }
    public string? Description { get; set; }
}