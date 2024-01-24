// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// Default implementation of <see cref="CefDownloadHandler"/>.
/// </summary>
public class DefaultDownloadHandler : CefDownloadHandler
{
    /// <inheritdoc/>
    protected override bool CanDownload(CefBrowser browser, string url, string request_method)
    {
        return true;
    }

    /// <inheritdoc/>
    protected override void OnBeforeDownload(CefBrowser browser, CefDownloadItem downloadItem, string suggestedName, CefBeforeDownloadCallback callback)
    {
        if (callback is not null)
        {
            using (callback)
            {
                callback.Continue(downloadItem.SuggestedFileName, showDialog: true);
            }
        }
    }
}