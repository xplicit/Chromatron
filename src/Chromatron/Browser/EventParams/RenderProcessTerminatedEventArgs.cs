// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// The render process terminated event args.
/// </summary>
public class RenderProcessTerminatedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenderProcessTerminatedEventArgs"/> class.
    /// </summary>
    /// <param name="status">
    /// The status.
    /// </param>
    public RenderProcessTerminatedEventArgs(CefTerminationStatus status)
    {
        Status = status;
    }

    /// <summary>
    /// Gets the status.
    /// </summary>
    public CefTerminationStatus Status { get; private set; }
}