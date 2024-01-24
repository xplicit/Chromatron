// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// The plugin crashed event args.
/// </summary>
public class PluginCrashedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginCrashedEventArgs"/> class.
    /// </summary>
    /// <param name="pluginPath">
    /// The plugin path.
    /// </param>
    public PluginCrashedEventArgs(string pluginPath)
    {
        PluginPath = pluginPath;
    }

    /// <summary>
    /// Gets the plugin path.
    /// </summary>
    public string PluginPath { get; }
}