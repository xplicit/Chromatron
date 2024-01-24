// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// Default CEF http scheme handler.
/// </summary>
/// <remarks>
/// Requests like those initiated by HTTP clients like jQuery, XMLHttpRequest, 
/// axios - https://github.com/axios/axios and alternatives will be processed here.
/// </remarks>
public class DefaultRequestSchemeHandler : ResourceHandler
{
    protected readonly IChromatronRouteProvider _routeProvider;
    protected readonly IChromatronRequestSchemeProvider _requestSchemeProvider;
    protected readonly IChromatronRequestHandler _requestHandler;
    protected readonly IChromatronDataTransferOptions _dataTransferOptions;
    protected readonly IChromatronErrorHandler _chromatronErrorHandler;

    protected IChromatronResponse _chromatronResponse;
    protected Stream _stream;
    protected string _mimeType;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultRequestSchemeHandler"/>.
    /// </summary>
    /// <param name="routeProvider">Instance of <see cref="IChromatronRouteProvider"/>.</param>
    /// <param name="requestSchemeProvider">Instance of <see cref="IChromatronRequestSchemeProvider"/>.</param>
    /// <param name="requestHandler">Instance of <see cref="IChromatronRequestHandler"/>.</param>
    /// <param name="dataTransferOptions">Instance of <see cref="IChromatronDataTransferOptions"/>.</param>
    /// <param name="chromatronErrorHandler">Instance of <see cref="IChromatronErrorHandler"/>.</param>
    public DefaultRequestSchemeHandler(IChromatronRouteProvider routeProvider,
                                       IChromatronRequestSchemeProvider requestSchemeProvider,
                                       IChromatronRequestHandler requestHandler,
                                       IChromatronDataTransferOptions dataTransferOptions,
                                       IChromatronErrorHandler chromatronErrorHandler)
    {
        _routeProvider = routeProvider;
        _requestSchemeProvider = requestSchemeProvider;
        _requestHandler = requestHandler;
        _dataTransferOptions = dataTransferOptions;
        _chromatronErrorHandler = chromatronErrorHandler;
        _chromatronResponse = new ChromatronResponse();
        _mimeType = ResourceHandler.DefaultMimeType;
        _stream = Stream.Null;
    }

    /// <inheritdoc/>
    public override CefReturnValue ProcessRequestAsync(CefRequest request, CefCallback callback)
    {
        var scheme = _requestSchemeProvider?.GetScheme(request.Url);
        if (scheme is not null && scheme.UrlSchemeType == UrlSchemeType.LocalRequest)
        {
            _stream = Stream.Null;
            var uri = new Uri(request.Url);
            var path = uri.LocalPath;
            _mimeType = "application/json";

            bool isRequestAsync = _routeProvider.IsRouteAsync(path);
            if (isRequestAsync)
            {
                ProcessRequestAsync(path);
            }
            else
            {
                ProcessRequest(path);
            }
        }

        return CefReturnValue.ContinueAsync;

        #region Process Request

        void ProcessRequest(string path)
        {
            Task.Run(() =>
            {
                using (callback)
                {
                    try
                    {
                        var response = new ChromatronResponse();
                        if (string.IsNullOrEmpty(path))
                        {
                            _chromatronResponse = _chromatronErrorHandler.HandleRouteNotFound(request.Identifier.ToString(), path);
                        }
                        else
                        {
                            var parameters = request.Url.GetParameters();
                            var postData = GetPostData(request);

                            var jsonRequest = _dataTransferOptions.ConvertObjectToJson(request);
                            _chromatronResponse = _requestHandler.Execute(request.Identifier.ToString(), path, parameters, postData, jsonRequest);
                            string? jsonData = _dataTransferOptions.ConvertResponseToJson(_chromatronResponse.Data);

                            if (jsonData is not null)
                            {
                                var content = Encoding.UTF8.GetBytes(jsonData);
                                _stream = new MemoryStream();
                                _stream.Write(content, 0, content.Length);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        _stream = Stream.Null;
                        var chromatronRequest = new ChromatronRequest() { Id = request.Identifier.ToString(), RouteUrl = request.Url };
                        _chromatronResponse = _chromatronErrorHandler.HandleError(chromatronRequest, exception);
                    }

                    if (_stream is null)
                    {
                        callback.Cancel();
                    }
                    else
                    {
                        SetResponseInfoOnSuccess();
                        callback.Continue();
                    }
                }
            });
        }

        #endregion

        #region Process Request Async

        void ProcessRequestAsync(string path)
        {
            Task.Run(async () =>
            {
                using (callback)
                {
                    try
                    {
                        var response = new ChromatronResponse();
                        if (string.IsNullOrEmpty(path))
                        {
                            _chromatronResponse = _chromatronErrorHandler.HandleRouteNotFound(request.Identifier.ToString(), path);
                        }
                        else
                        {
                            var parameters = request.Url.GetParameters(request.ReferrerURL);
                            var postData = GetPostData(request);

                            var jsonRequest = _dataTransferOptions.ConvertObjectToJson(request);
                            _chromatronResponse = await _requestHandler.ExecuteAsync(request.Identifier.ToString(), path, parameters, postData, jsonRequest);
                            string? jsonData = _dataTransferOptions.ConvertResponseToJson(_chromatronResponse.Data);

                            if (jsonData is not null)
                            {
                                var content = Encoding.UTF8.GetBytes(jsonData);
                                _stream = new MemoryStream();
                                await _stream.WriteAsync(content, 0, content.Length);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        _stream = Stream.Null;
                        var chromatronRequest = new ChromatronRequest() { Id = request.Identifier.ToString(), RouteUrl = request.Url };
                        _chromatronResponse = _chromatronErrorHandler.HandleError(chromatronRequest, exception);
                    }

                    if (_stream is null)
                    {
                        callback.Cancel();
                    }
                    else
                    {
                        SetResponseInfoOnSuccess();
                        callback.Continue();
                    }
                }
            });
        }

        #endregion
    }

    protected virtual void SetResponseInfoOnSuccess()
    {
        //Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
        _stream.Position = 0;
        //Populate the response values - No longer need to implement GetResponseHeaders (unless you need to perform a redirect)
        ResponseLength = _stream.Length;
        MimeType = _mimeType;
        StatusCode = _chromatronResponse.Status;
        StatusText = _chromatronResponse.StatusText;
        Stream = _stream;
        MimeType = _mimeType;

        if (Headers is not null)
        {
            Headers.Add("Cache-Control", "private");
            Headers.Add("Access-Control-Allow-Methods", "GET,POST");
            Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            Headers.Add("Content-Type", "application/json; charset=utf-8");
        }
    }

    private static string GetPostData(CefRequest request)
    {
        var postDataElements = request?.PostData?.GetElements();
        if (postDataElements is null || (postDataElements.Length == 0))
        {
            return string.Empty;
        }

        var dataElement = postDataElements[0];

        switch (dataElement.ElementType)
        {
            case CefPostDataElementType.Empty:
                break;
            case CefPostDataElementType.File:
                break;
            case CefPostDataElementType.Bytes:
                return Encoding.UTF8.GetString(dataElement.GetBytes());
        }

        return string.Empty;
    }
}