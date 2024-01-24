// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Logging;

/// <summary>
/// Ambient context class to manage instance of <see cref="ILogger"/>.
/// </summary>
public abstract class Logger
{
    private static Logger? instance;

    /// <summary>
    /// The <see cref="ILogger"/> instance.
    /// </summary>
    public static Logger Instance
    {
        get
        {
            if (instance is null)
            {
                // Ambient Context can't return null, so we assign Local Default
                instance = new DefaultLogger();
            }

            return instance;
        }
        set
        {
            instance = (value is null) ? new DefaultLogger() : value;
        }
    }
#nullable disable

    /// <summary>
    /// Gets or sets the application logger.
    /// </summary>
    public virtual ILogger Log { get; set; }
#nullable restore
}
