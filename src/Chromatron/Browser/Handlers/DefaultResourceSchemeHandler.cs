// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// Default implementation of <see cref="ResourceHandler"/>. 
/// At a minimum developer only need to override ProcessRequestAsync.
/// </summary>
public class DefaultResourceSchemeHandler : ResourceHandler
{
    protected readonly IChromatronConfiguration _config;
    protected readonly IChromatronErrorHandler _chromatronErrorHandler;
    protected IChromatronResource _chromatronResource;
    protected FileInfo? _fileInfo;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultResourceSchemeHandler"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    /// <param name="chromatronErrorHandler">Instance of <see cref="IChromatronErrorHandler"/>.</param>
    public DefaultResourceSchemeHandler(IChromatronConfiguration config, IChromatronErrorHandler chromatronErrorHandler)
    {
        _config = config;
        _chromatronErrorHandler = chromatronErrorHandler;
        _chromatronResource = new ChromatronResource();
        _fileInfo = null;
    }

    /// <summary>
    /// The process request async.
    /// </summary>
    /// <param name="request">
    /// The request.
    /// </param>
    /// <param name="callback">
    /// The callback.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    public override CefReturnValue ProcessRequestAsync(CefRequest request, CefCallback callback)
    {
        var uri = new Uri(request.Url);
        var scheme = _config.UrlSchemes.GetScheme(request.Url);
        var isFolderResourceScheme = scheme is not null && scheme.IsUrlSchemeFolderResource();

        var u = new Uri(request.Url);
        var file = isFolderResourceScheme
                    ? scheme.GetResourceFolderFile(u.AbsolutePath)
                    : u.Authority + u.AbsolutePath;

        _fileInfo = new FileInfo(file);
        // Check if file exists 
        if (!_fileInfo.Exists)
        {
            _chromatronResource = _chromatronErrorHandler.HandleError(_fileInfo);
            callback.Continue();
        }
        // Check if file exists but empty
        else if (_fileInfo.Length == 0)
        {
            _chromatronResource = _chromatronErrorHandler.HandleError(_fileInfo);
            callback.Continue();
        }
        else
        {
            Task.Run(() =>
            {
                using (callback)
                {
                    _chromatronResource.Content = null;
                    _chromatronResource.MimeType = "text/html";

                    try
                    {
                        byte[] fileBytes = File.ReadAllBytes(file);
                        _chromatronResource.Content = new MemoryStream(fileBytes);

                        string extension = Path.GetExtension(file);
                        _chromatronResource.MimeType = MimeMapper.GetMimeType(extension);
                        _chromatronResource.StatusCode = ResourceConstants.StatusOK;
                        _chromatronResource.StatusText = ResourceConstants.StatusOKText;
                    }
                    catch (Exception exception)
                    {
                        _chromatronResource = _chromatronErrorHandler.HandleError(_fileInfo, exception);
                    }

                    if (_chromatronResource.Content is null)
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

        return CefReturnValue.ContinueAsync;
    }

    protected virtual void SetResponseInfoOnSuccess()
    {
        Stream = Stream.Null;

        //Reset the stream position to 0 so the stream can be copied into the underlying unmanaged buffer
        if (_chromatronResource.Content is not null)
        {
            _chromatronResource.Content.Position = 0;
            //Populate the response values - No longer need to implement GetResponseHeaders (unless you need to perform a redirect)
            ResponseLength = _chromatronResource.Content.Length;
            Stream = _chromatronResource.Content;
        }

        MimeType = _chromatronResource.MimeType;
        StatusCode = (int)_chromatronResource.StatusCode;
        StatusText = _chromatronResource.StatusText;
    }
}