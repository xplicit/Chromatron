// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Network;

/// <summary>
/// The Chromatron request.
/// </summary>
public interface IChromatronRequest
{
    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    string Id { get; set; }
    string Name { get; set; }

    /// <summary>
    /// Gets or sets the route url.
    /// </summary>
    string RouteUrl { get; set; }

    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    IDictionary<string, object>? Parameters { get; set; }

    /// <summary>
    /// Gets or sets the post data.
    /// </summary>
    object? PostData { get; set; }

    /// <summary>
    /// Gets or sets the raw json.
    /// Only used for CefGlue Generic Message Routing requests.
    /// </summary>
    string? RawJson { get; set; }
}