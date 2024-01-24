// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core;

/// <summary>
/// Sets of error handling functions.
/// </summary>
public interface IChromatronErrorHandler
{
    /// <summary>
    /// Handle route path not found error.
    /// </summary>
    /// <param name="requestId">Request identifier.</param>
    /// <param name="routePath">The controller route path.</param>
    /// <returns>Instance of <see cref="IChromatronResponse"/>.</returns>
    IChromatronResponse HandleRouteNotFound(string requestId, string routePath);

    /// <summary>
    /// Handle resource request error.
    /// </summary>
    /// <param name="fileInfo">The resource file - instance of <see cref="FileInfo"/>. </param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromatronResponse"/>.</returns>
    IChromatronResource HandleError(FileInfo? fileInfo, Exception? exception = null);

    /// <summary>
    /// Handle resource request error.
    /// </summary>
    /// <param name="stream">The resource file - instance of <see cref="Stream"/>. </param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromatronResponse"/>.</returns>
    IChromatronResource HandleError(Stream? stream, Exception? exception = null);

    /// <summary>
    /// Handle request error.
    /// </summary>
    /// <param name="request">The request - instance of <see cref="IChromatronRequest"/>.</param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromatronResponse"/>.</returns>
    IChromatronResponse HandleError(IChromatronRequest request, Exception? exception = null);

    /// <summary>
    /// Handle request error.
    /// </summary>
    /// <param name="request">The request - instance of <see cref="IChromatronRequest"/>.</param>
    /// <param name="response">The response - instance of <see cref="IChromatronResponse"/>.</param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromatronResponse"/>.</returns>
    IChromatronResponse HandleError(IChromatronRequest request, IChromatronResponse response, Exception? exception = null);

    /// <summary>
    /// Handle request error asynchronously.
    /// </summary>
    /// <param name="requestUrl">The controller route url path.</param>
    /// <param name="response">The response - instance of <see cref="IChromatronResponse"/>.</param>
    /// <param name="exception">Instance of <see cref="Exception"/>.</param>
    /// <returns>Instance of <see cref="IChromatronResponse"/>.</returns>
    Task<IChromatronResource> HandleErrorAsync(string requestUrl, IChromatronResource response, Exception? exception = null);
}
