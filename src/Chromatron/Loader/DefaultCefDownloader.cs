// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Loader;

/// <inheritdoc/>
public class DefaultCefDownloader : ICefDownloader
{
    protected readonly IChromatronConfiguration _config;
    protected ICefDownloadNotification _notification;

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultCefDownloader"/>.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    public DefaultCefDownloader(IChromatronConfiguration config)
    {
        _config = config;
        _notification = null;
    }

    /// <inheritdoc/>
    public ICefDownloadNotification Notification
    {
        get
        {
            if (_notification is null)
            {
                switch (_config.CefDownloadOptions.NotificationType)
                {
                    case CefDownloadNotificationType.Logger:
                        _notification = new LoggerCefDownloadNotification();
                        break;

                    case CefDownloadNotificationType.Console:
                        _notification = new ConsoleCefDownloadNotification();
                        break;

                    case CefDownloadNotificationType.HTML:
                        _notification = new HtmlCefDownloadNotification();
                        break;

                    default:
                        _notification = new LoggerCefDownloadNotification();
                        break;
                }
            }

            return _notification;
        }
    }

    /// <inheritdoc/>
    public void Download(IChromatronConfiguration config)
    {
        CefLoader.Download(config.Platform);
    }
}