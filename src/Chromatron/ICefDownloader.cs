// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron;

/// <summary>
/// Represents CEF binaries downloader.
/// </summary>
public interface ICefDownloader
{
    /// <summary>
    /// Gets the CEF download notification - instance of <see cref="ICefDownloadNotification"/>.
    /// </summary>
    ICefDownloadNotification Notification { get; }

    /// <summary>
    /// Primary entry function for downloading CEF binaries.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    void Download(IChromatronConfiguration config);
}