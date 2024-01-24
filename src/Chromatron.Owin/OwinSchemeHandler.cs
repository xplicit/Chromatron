// Copyright (c) Alex Maitland. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Chromatron.Owin;

/// <summary>
/// Loosly based on https://github.com/eVisionSoftware/Harley/blob/master/src/Harley.UI/Owin/OwinSchemeHandlerFactory.cs
/// New instance is instanciated for every request
/// </summary>
public class OwinSchemeHandler : ResourceHandler
{
    protected readonly IOwinPipeline _owinPipeline;
    protected readonly IChromatronErrorHandler _errorHandler;

    /// <summary>
    /// Initializes a new instance of <see cref="OwinSchemeHandler"/>.
    /// </summary>
    /// <param name="owinPipeline">Owin pipeline type of <see cref="IOwinPipeline"/>. </param>
    /// <param name="errorHandler">Error handler type of <see cref="IChromatronErrorHandler"/>. </param>
    public OwinSchemeHandler(IOwinPipeline owinPipeline, IChromatronErrorHandler errorHandler)
    {
        _owinPipeline = owinPipeline;
        _errorHandler = errorHandler;
    }

    /// <inheritdoc/>
    public override CefReturnValue ProcessRequestAsync(CefRequest request, CefCallback callback)
    {
        var requestBody = Stream.Null;
        if (request.Method == "POST")
        {
            using var postData = request.PostData;
            if (postData is not null)
            {
                var postDataElements = postData.GetElements();
                var firstPostDataElement = postDataElements.First();
                var bytes = firstPostDataElement.GetBytes();
                requestBody = new MemoryStream(bytes, 0, bytes.Length);
            }
        }

        var uri = new Uri(request.Url);
        var requestHeaders = request.GetHeaderMap();
        //Add Host header as per http://owin.org/html/owin.html#5-2-hostname
        requestHeaders.Add("Host", uri.Host + (uri.Port > 0 ? (":" + uri.Port) : ""));

        Task.Run(async () =>
        {
            IChromatronResource chromatronResource = new ChromatronResource();

            try
            {
                // Call into the OWIN pipeline
                var owinRequest = new ResourceRequest(request.Url, request.Method, requestHeaders, requestBody);
                var owinResponse = await RequestInterceptor.ProcessRequest(_owinPipeline.AppFunc, owinRequest);

                chromatronResource =  new ChromatronResource()
                {
                    Content = owinResponse.Stream as MemoryStream,
                    MimeType = owinResponse.Headers.GetMimeType(),
                    StatusCode = (HttpStatusCode)owinResponse.StatusCode,
                    StatusText = owinResponse.ReasonPhrase,
                    Headers = owinResponse.Headers
                };

                if (chromatronResource.StatusCode.IsClientErrorCode() || chromatronResource.StatusCode.IsServerErrorCode())
                {
                    chromatronResource = await _errorHandler.HandleErrorAsync(request.Url, chromatronResource, null);
                }
            }
            catch (Exception exception)
            {
                chromatronResource = await _errorHandler.HandleErrorAsync(request.Url, chromatronResource, exception);
            }

            //Populate the response properties
            Stream = chromatronResource.Content ?? Stream.Null;
            ResponseLength = (chromatronResource.Content ==  null ) ? 0 : chromatronResource.Content.Length;
            StatusCode = (int)chromatronResource.StatusCode;
            MimeType = chromatronResource.MimeType;

            foreach (var header in chromatronResource.Headers)
            {
                //It's possible for headers to have multiple values
                foreach (var val in header.Value)
                {
                    Headers.Add(header.Key, val);
                }
            }

            //Once we've finished populating the properties we execute the callback
            //Callback wraps an unmanaged resource, so let's explicitly Dispose when we're done    
            using (callback)
            {
                callback.Continue();
            }
        });

        return CefReturnValue.ContinueAsync;
    }
}