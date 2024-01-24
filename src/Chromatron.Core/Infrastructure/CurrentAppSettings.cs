// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Infrastructure;

/// <summary>
/// Wrapper class to assign <see cref="DefaultAppSettings"/> or a custom implementation of <see cref="IChromatronAppSettings"/>.
/// </summary>
public class CurrentAppSettings : ChromatronAppUser
{
    private IChromatronAppSettings? appSettings;

    /// <summary>
    /// Gets or sets the dynamic object as a set of "Properties".
    /// </summary>
    public override IChromatronAppSettings Properties
    {
        get
        {
            if (appSettings is null)
            {
                appSettings = new DefaultAppSettings();
            }

            return appSettings;
        }
        set
        {
            appSettings = value;
        }
    }
}
