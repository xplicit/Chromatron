// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Configuration;

public enum CefDownloadNotificationType
{
    /// <summary>
    /// None - defaults to Logger if not set.
    /// </summary>
    None,

    /// <summary>
    /// Logs notification to application Logger set.
    /// </summary>
    Logger,

    /// <summary>
    /// Logs notification to Console output.
    /// </summary>
    Console,

    /// <summary>
    /// Uses html pages to display notification.
    /// </summary>
    HTML,

    /// <summary>
    /// Other options - for developer use.
    /// </summary>
    Custom
}

