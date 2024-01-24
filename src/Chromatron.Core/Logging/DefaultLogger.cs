// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Logging;

/// <summary>
/// Wrapper class to assign <see cref="SimpleLogger"/> or a custom implementation of <see cref="ILogger"/>.
/// </summary>
public class DefaultLogger : Logger
{
    private ILogger? _ChromatronLogger;

    /// <summary>
    /// Gets or sets logger.
    /// </summary>
    public override ILogger Log
    {
        get
        {
            if (_ChromatronLogger is null)
            {
                _ChromatronLogger = new SimpleLogger();
            }

            return _ChromatronLogger;
        }
        set
        {
            _ChromatronLogger = value;
        }
    }
}