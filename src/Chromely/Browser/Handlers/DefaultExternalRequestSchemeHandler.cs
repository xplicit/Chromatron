// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#nullable disable
#pragma warning disable CA2016

using System.Net.Http.Headers;

namespace Chromely.Browser;

/// <summary>
/// Loads external HTTP request resources like proxy.
/// Omits X-Frame-Options headers and adds Access-Control-Allow-Origin: * header
/// </summary>
public class DefaultExternalRequestSchemeHandler : DefaultAsyncHandlerBase
{
    protected readonly HttpClient _httpClient;
    protected HttpRequestMessage _httpRequest;
    protected HttpResponseMessage _httpResponseMessage;
    protected long _responseLength;

    /// <summary>
    /// Initializes a new instance of the Chromely.CefGlue.Browser.Handlers.ExternalRequestSchemeHandler class.
    /// </summary>
    /// <param name="httpClient">HttpClient object that will be used for requests</param>
    public DefaultExternalRequestSchemeHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Converts http request method name into object.
    /// </summary>
    protected virtual HttpMethod GetHttpMethod(string methodName)
    {
        return methodName.ToUpperInvariant() switch
        {
            "GET" => HttpMethod.Get,
            "PUT" => HttpMethod.Put,
            "POST" => HttpMethod.Post,
            "DELETE" => HttpMethod.Delete,
            "HEAD" => HttpMethod.Head,
            "OPTIONS" => HttpMethod.Options,
            "TRACE" => HttpMethod.Trace,
            "PATCH" => new HttpMethod("PATCH"),
            "CONNECT" => new HttpMethod("CONNECT"),
            _ => throw new ArgumentException($"Unknown http method: {methodName}"),
        };
    }

    /// <summary>
    /// Builds http request message from cef request
    /// </summary>
    /// <param name="cefRequest"></param>
    protected virtual HttpRequestMessage BuildHttpRequest(CefRequest cefRequest)
    {
        var httpRequest = new HttpRequestMessage(GetHttpMethod(cefRequest.Method), cefRequest.Url);

        var cefHeaders = cefRequest.GetHeaderMap();
        foreach (var key in cefHeaders.AllKeys)
        {
            httpRequest.Headers.TryAddWithoutValidation(key, cefHeaders.GetValues(key));
        }

        // Should not be set as header, it is handled by CookieContainer
        httpRequest.Headers.Remove(RequestConstants.Header_Cookie);

        if (cefRequest.ReferrerURL is not null && Uri.TryCreate(cefRequest.ReferrerURL, UriKind.Absolute, out var referrerUri))
        {
            httpRequest.Headers.Referrer = referrerUri;
        }

        if (cefRequest.PostData is not null && cefRequest.PostData.Count > 0)
        {
            var postDataElements = cefRequest.PostData.GetElements();
            var postDataStream = new PostDataStream(postDataElements);
            httpRequest.Content = new StreamContent(postDataStream);

            var contentType = cefRequest.GetHeaderByName(RequestConstants.Header_ContentType);
            if (MediaTypeHeaderValue.TryParse(contentType, out var mediaTypeHeaderValue))
            {
                httpRequest.Content.Headers.ContentType = mediaTypeHeaderValue;
                httpRequest.Content.Headers.ContentLength = postDataElements.Sum(e => e.BytesCount);
            }
        }

        return httpRequest;
    }

    /// <inheritdoc/>
    protected override bool PrepareRequest(CefRequest request)
    {
        _httpRequest = BuildHttpRequest(request);
        return true;
    }

    /// <inheritdoc/>
    protected override async Task<bool> LoadResourceData(CancellationToken cancellationToken)
    {
        _httpResponseMessage = await _httpClient.SendAsync(_httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        return _httpResponseMessage.Content is not null;
    }
    /// <inheritdoc/>
    protected override Task<Stream> GetResourceDataStream(CancellationToken cancellationToken)
    {
        return _httpResponseMessage.Content.ReadAsStreamAsync();
    }

    /// <inheritdoc/>
    protected override void GetResponseHeaders(CefResponse response, out long responseLength, out string redirectUrl)
    {
        // unknown content-length
        // no-redirect
        responseLength = -1;
        redirectUrl = null;

        try
        {
            if (_httpResponseMessage is null)
            {
                response.Error = CefErrorCode.Failed;
                return;
            }

            var headers = response.GetHeaderMap();
            headers.Clear();

            this.ProcessHeaders(_httpResponseMessage, headers);
            response.SetHeaderMap(headers);

            response.MimeType = _httpResponseMessage.Content?.Headers?.ContentType?.MediaType;
            response.Status = (int)_httpResponseMessage.StatusCode;
            response.StatusText = _httpResponseMessage.ReasonPhrase;
            responseLength = this._responseLength = _httpResponseMessage.Content?.Headers?.ContentLength ?? -1;

            if (_httpResponseMessage.StatusCode == HttpStatusCode.MovedPermanently
                || _httpResponseMessage.StatusCode == HttpStatusCode.Moved
                || _httpResponseMessage.StatusCode == HttpStatusCode.Redirect
                || _httpResponseMessage.StatusCode == HttpStatusCode.RedirectMethod
                || _httpResponseMessage.StatusCode == HttpStatusCode.TemporaryRedirect)
                redirectUrl = _httpResponseMessage.Headers.Location.ToString();

        }
        catch (Exception ex)
        {
            response.Error = CefErrorCode.Failed;
            Logger.Instance.Log.LogError(ex, "Exception thrown while processing request");
        }
    }

    /// <inheritdoc/>
    protected virtual void ProcessHeaders(HttpResponseMessage httpResponseMessage, NameValueCollection headers)
    {
        foreach (var header in _httpResponseMessage.Headers)
        {
            if (header.Key == "X-Frame-Options")
                continue;
            foreach (var val in header.Value)
                headers.Add(header.Key, val);
        }

        if (_httpResponseMessage.Content is not null)
        {
            foreach (var header in _httpResponseMessage.Content.Headers)
            {
                foreach (var val in header.Value)
                    headers.Add(header.Key, val);
            }
        }
    }

    /// <inheritdoc/>
    protected override long GetDataSize()
    {
        return this._responseLength;
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        this._httpRequest?.Dispose();
        this._httpResponseMessage?.Dispose();

        this._httpRequest = null;
        this._httpResponseMessage = null;

        base.Dispose(disposing);
    }
}
