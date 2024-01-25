// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Infrastructure;

/// <summary>
/// This class provides operating system and runtime information
/// used to host the application.
/// </summary>
public static class ChromatronRuntime
{
    /// <summary>
    /// Gets the runtime the application is running on.
    /// </summary>
    public static ChromatronPlatform Platform
    {
        get
        {
            return Environment.OSVersion.Platform switch
            {
                PlatformID.MacOSX => ChromatronPlatform.MacOSX,
                PlatformID.Unix or (PlatformID)128 => IsRunningOnMac()
                                                     ? ChromatronPlatform.MacOSX
                                                     : ChromatronPlatform.Linux,
                PlatformID.Win32NT or PlatformID.Win32S or PlatformID.Win32Windows or PlatformID.WinCE or PlatformID.Xbox => ChromatronPlatform.Windows,
                _ => ChromatronPlatform.NotSupported,
            };
        }
    }

    private static bool IsRunningOnMac()
    {
        try
        {
            var osName = Environment.OSVersion.VersionString;
            if (osName.ToLowerInvariant().Contains("darwin")) return true;
            if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist")) return true;
        }
        catch { }

        return false;
    }
}