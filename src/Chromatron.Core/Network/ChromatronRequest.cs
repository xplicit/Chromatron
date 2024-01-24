// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Network;

/// <summary>
/// The Chromatron request.
/// </summary>
public class ChromatronRequest : IChromatronRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChromatronRequest"/> class.
    /// </summary>
    public ChromatronRequest()
    {
        Name = Guid.NewGuid().ToString();
        Id = Guid.NewGuid().ToString();
        RouteUrl = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromatronRequest"/> class.
    /// </summary>
    /// <param name="routeUrl">
    /// The route path.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <param name="postData">
    /// The post data.
    /// </param>
    public ChromatronRequest(string routeUrl, IDictionary<string, object>? parameters, object? postData)
        : this()
    {
        RouteUrl = routeUrl;
        Parameters = parameters;
        PostData = postData;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromatronRequest"/> class.
    /// </summary>
    /// <param name="id">
    /// The id.
    /// </param>
    /// <param name="routeUrl">
    /// The route path.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <param name="postData">
    /// The post data.
    /// </param>
    public ChromatronRequest(string id, string routeUrl, IDictionary<string, object>? parameters, object? postData)
        : this()
    {
        Id = id;
        RouteUrl = routeUrl;
        Parameters = parameters;
        PostData = postData;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromatronRequest"/> class.
    /// </summary>
    /// <param name="id">
    /// The id.
    /// </param>
    /// <param name="routeUrl">
    /// The route path.
    /// </param>
    /// <param name="parameters">
    /// The parameters.
    /// </param>
    /// <param name="postData">
    /// The post data.
    /// </param>
    /// <param name="rawJson">
    /// The raw json.
    /// </param>
    public ChromatronRequest(string id, string routeUrl, IDictionary<string, object>? parameters, object? postData, string? rawJson)
        : this()
    {
        Id = id;
        RouteUrl = routeUrl;
        Parameters = parameters;
        PostData = postData;
        RawJson = rawJson;
    }

    /// <summary>
    /// Gets or sets the id.
    /// </summary>
    public string Id { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the route path.
    /// </summary>
    public string RouteUrl { get; set; }

    /// <summary>
    /// Gets or sets the parameters.
    /// </summary>
    public IDictionary<string, object>? Parameters { get; set; }

    /// <summary>
    /// Gets or sets the post data.
    /// </summary>
    public object? PostData { get; set; }

    /// <summary>
    /// Gets or sets the raw json.
    /// Only used for CefGlue Generic Message Routing requests.
    /// </summary>
    public string? RawJson { get; set; }
}