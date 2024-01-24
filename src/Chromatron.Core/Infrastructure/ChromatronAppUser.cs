// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Infrastructure;

/// <summary>
/// Ambient context class to manage instance of <see cref="IChromatronAppSettings"/>.
/// </summary>
public abstract class ChromatronAppUser
{
    private static ChromatronAppUser? instance;

    /// <summary>
    /// The <see cref="IChromatronAppSettings"/> instance.
    /// </summary>
    public static ChromatronAppUser App
    {
        get
        {
            if (instance is null)
            {
                // Ambient Context can't return null, so we assign Local Default
                instance = new CurrentAppSettings();
            }

            return instance;
        }
        set
        {
            instance = (value is null) ? new CurrentAppSettings() : value;
        }
    }

    /// <summary>
    /// Gets or sets the application settings.
    /// </summary>
    public virtual IChromatronAppSettings Properties { get; set; } = new DefaultAppSettings();
}
