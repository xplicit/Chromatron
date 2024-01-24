// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Defaults;

/// <summary>
/// The default implementation of <see cref="IChromatronErrorHandler"/>.
/// </summary>
public class DefaultErrorHandler : IChromatronErrorHandler
{
    /// <inheritdoc/>
    public virtual IChromatronResponse HandleRouteNotFound(string requestId, string routePath)
    {
        return new ChromatronResponse
        {
            RequestId = requestId,
            ReadyState = (int)ReadyState.ResponseIsReady,
            Status = (int)System.Net.HttpStatusCode.BadRequest,
            StatusText = $"Route for path = {routePath} is null or invalid."
        };
    }

    /// <inheritdoc/>
    public virtual IChromatronResource HandleError(FileInfo? fileInfo, Exception? exception = null)
    {
        var info = DefaultErrorHandler.GetFileInfo(fileInfo);
        bool fileExists = info.Item1;
        int fileSize = info.Item2;

        var resource = DefaultErrorHandler.HandleResourceError(fileExists, fileSize, exception);
        Logger.Instance.Log.LogWarning("File: {fileInfo?.FullName}: {resource?.StatusText}", fileInfo?.FullName, resource?.StatusText);
        return resource;
    }

    /// <inheritdoc/>
    public IChromatronResource HandleError(Stream? stream, Exception? exception = null)
    {
        var info = DefaultErrorHandler.GetFileInfo(stream);
        bool fileExists = info.Item1;
        int fileSize = info.Item2;

        return DefaultErrorHandler.HandleResourceError(fileExists, fileSize, exception);
    }

    /// <inheritdoc/>
    public virtual IChromatronResponse HandleError(IChromatronRequest request, Exception? exception = null)
    {
        if (exception is not null)
        {
            Logger.Instance.Log.LogError(exception);
        }

        var localResponse = new ChromatronResponse
        {
            ReadyState = (int)ReadyState.ResponseIsReady,
            Status = (int)System.Net.HttpStatusCode.BadRequest,
            StatusText = "An error has occurred"
        };

        localResponse.RequestId = (request is not null && string.IsNullOrWhiteSpace(request.Id))
                                ? request.Id
                                : localResponse.RequestId;

        return localResponse;
    }

    /// <inheritdoc/>
    public virtual IChromatronResponse HandleError(IChromatronRequest request, IChromatronResponse response, Exception? exception = null)
    {
        if (exception is not null)
        {
            Logger.Instance.Log.LogError(exception);
        }

        var localResponse = new ChromatronResponse
        {
            ReadyState = (int)ReadyState.ResponseIsReady,
            Status = (int)System.Net.HttpStatusCode.BadRequest,
            StatusText = "An error has occurred"
        };

        localResponse.RequestId = (request is not null && string.IsNullOrWhiteSpace(request.Id))
                                ? request.Id
                                : localResponse.RequestId;

        return localResponse;
    }

    /// <inheritdoc/>
    public virtual Task<IChromatronResource> HandleErrorAsync(string requestUrl, IChromatronResource response, Exception? exception = null)
    {
        return Task.FromResult<IChromatronResource>(response);
    }

    private static IChromatronResource HandleResourceError(bool fileExists, int fileSize, Exception? exception = null)
    {
        if (exception is not null)
        {
            Logger.Instance.Log.LogError(exception);
        }

        var resource = new ChromatronResource();
        if (!fileExists)
        {
            resource.StatusCode = HttpStatusCode.NotFound;
            resource.StatusText = "Resource loading error: file size is zero.";
            resource.Content = resource.StatusText.GetMemoryStream();
        }

        else if (fileSize == 0)
        {
            resource.StatusCode = HttpStatusCode.NotFound;
            resource.StatusText = "Resource loading error: file size is zero.";
            resource.Content = resource.StatusText.GetMemoryStream();
        }
        else
        {
            resource.StatusCode = HttpStatusCode.BadRequest;
            resource.StatusText = "Resource loading error";
            resource.Content = resource.StatusText.GetMemoryStream();
        }

        return resource;
    }

    private static (bool, int) GetFileInfo(object? infoOrStream)
    {
        bool fileExists = false;
        int fileSize = 0;

        try
        {
            var fileInfo = infoOrStream as FileInfo;
            if (fileInfo is not null)
            {
                fileExists = fileInfo is not null && fileInfo.Exists;
                fileSize = (int)(fileInfo is not null ? fileInfo.Length : 0);
            }

            var stream = infoOrStream as Stream;
            if (stream is not null)
            {
                fileExists = stream is not null;
                fileSize = (int)(stream is not null ? stream.Length : 0);
            }

            return (fileExists, fileSize);
        }
        catch { }

        return (fileExists, fileSize);
    }
}
