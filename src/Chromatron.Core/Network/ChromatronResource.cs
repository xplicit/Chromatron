// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Network;

/// <summary>
/// Default implementation of <see cref="ChromatronResource"/>.
/// </summary>
public class ChromatronResource : IChromatronResource
{
    /// <summary>
    /// Initializes a new instance of <see cref="ChromatronResource"/>.
    /// </summary>
    public ChromatronResource()
    {
        StatusCode = ResourceConstants.StatusOK;
        StatusText = ResourceConstants.StatusOKText;
        MimeType = "text/plain";
        Headers = new Dictionary<string, string[]>();
    }

    /// <inheritdoc/>
    public MemoryStream? Content { get; set; }

    /// <inheritdoc/>
    public string MimeType { get; set; }

    /// <inheritdoc/>
    public HttpStatusCode StatusCode { get; set; }

    /// <inheritdoc/>
    public string StatusText { get; set; }

    /// <inheritdoc/>
    public IDictionary<string, string[]> Headers { get; set; }
}