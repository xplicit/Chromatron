// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Owin;

public class ChromatronOwinErrorHandler : DefaultErrorHandler
{
    protected readonly IChromatronConfiguration _config;
    protected readonly IOwinPipeline _owinPipeline;

    /// <summary>
    /// Initializes a new instance of <see cref="OwinErrorHandler"/>.
    /// </summary>
    /// <param name="config">Configuration <see cref="IChromatronConfiguration"/> instance.</param>
    /// <param name="owinPipeline">The <see cref="IOwinPipeline"/> instance.</param>
    public ChromatronOwinErrorHandler(IChromatronConfiguration config, IOwinPipeline owinPipeline)
    {
        _config = config;
        _owinPipeline = owinPipeline;
    }

    /// <inheritdoc/>
    public async override Task<IChromatronResource> HandleErrorAsync(string requestUrl, IChromatronResource response, Exception? exception)
    {
        if (exception is not null)
        {
            Logger.Instance.Log.LogError(exception);
        }

        if (_owinPipeline.IsUrlActionRoute(requestUrl) && !_owinPipeline.IsUrlErrorHandlingPath(requestUrl))
        {
            string errorPageUrl = GetErrorPageUrl(requestUrl, _config.StartUrl);
            var newOwinRequest = new ResourceRequest(errorPageUrl, "GET", new Dictionary<string, string[]>(), null);
            var owinResponse = await RequestInterceptor.ProcessRequest(_owinPipeline.AppFunc, newOwinRequest);

            return new ChromatronResource()
            {
                Content = owinResponse.Stream as MemoryStream,
                MimeType = owinResponse.Headers.GetMimeType(),
                StatusCode = (HttpStatusCode)owinResponse.StatusCode,
                StatusText = owinResponse.ReasonPhrase,
                Headers = owinResponse.Headers
            };
        }

        return response;
    }

    private string GetErrorPageUrl(string url, string startUrl)
    {
        var refererUri = CreateUri(url, startUrl);
        return $"{refererUri?.Scheme}{Uri.SchemeDelimiter}{refererUri?.Host}{refererUri?.Port}{_owinPipeline.ErrorHandlingPath}";
    }

    private static Uri? CreateUri(string url, string startUrl)
    {
        try
        {
            return new Uri(url);
        }
        catch { }

        try
        {
            return new Uri(startUrl);
        }
        catch { }

        return default;
    }
}