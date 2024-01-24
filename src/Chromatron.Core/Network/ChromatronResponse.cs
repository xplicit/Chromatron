// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Network;

/// <summary>
/// Default implementation of <see cref="IChromatronResponse"/>.
/// </summary>
public class ChromatronResponse : IChromatronResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChromatronResponse"/> class.
    /// </summary>
    public ChromatronResponse()
    {
        RequestId = Guid.NewGuid().ToString();
        Error = string.Empty;
        Status = ResponseConstants.StatusOK;
        StatusText = ResponseConstants.StatusOKText;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChromatronResponse"/> class.
    /// </summary>
    /// <param name="requestId">
    /// The request id.
    /// </param>
    public ChromatronResponse(string requestId)
        : this()
    {
        RequestId = requestId;
    }

    /// <summary>
    /// Gets or sets the route path.
    /// </summary>
    public string RequestId { get; set; }

    /// <summary>
    /// Gets or sets the ready state.
    /// </summary>
    public int ReadyState { get; set; }

    /// <summary>
    /// Gets or sets the status.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// Gets or sets the status text.
    /// </summary>
    public string StatusText { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    public object? Data { get; set; }

    /// <inheritdoc/>
    public bool HasRouteResponse { get; set; }

    /// <inheritdoc/>
    public bool HasError
    {
        get
        {
            return !string.IsNullOrWhiteSpace(Error) || Status != (int)System.Net.HttpStatusCode.OK;
        }
    }

    /// <inheritdoc/>
    public string Error { get; set; }
}