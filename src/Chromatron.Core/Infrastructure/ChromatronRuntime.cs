// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Infrastructure;

/// <summary>
/// This class provides operating system and runtime information
/// used to host the application.
/// </summary>
public static class ChromatronRuntime
{
    /// <summary>The get expected chromium build number.</summary>
    /// <returns>The <see cref="int" />.</returns>
    public static CefBuildNumbers GetExpectedCefBuild()
    {
        try
        {
            //TODO: rework this, make DI. Currently we have magic properties
            //which must be handled with care when upgrading CefGlue.
            //We should not depend on the CefGlue properties names
            //nor library name
#nullable disable
            var appExeLocation = AppDomain.CurrentDomain.BaseDirectory;
            string dllName = Path.Combine(appExeLocation, "Chromatron.dll");
            var assembly = Assembly.LoadFrom(dllName);
            var types = assembly.GetTypes();
            var type = types.FirstOrDefault(t => t.Name == "CefRuntime");
            var versionProperty = type?.GetProperty("CefVersion");
            var cefVersion = versionProperty?.GetValue(null).ToString();
            versionProperty = type?.GetProperty("ChromeVersion");
            var chromiumVersion = versionProperty?.GetValue(null).ToString();
            return new CefBuildNumbers(cefVersion, chromiumVersion);
#nullable restore
        }
        catch (Exception ex)
        {
            Logger.Instance.Log.LogError("Could not get expected chromium build number: {ex.Message}", ex.Message);
        }
        return new CefBuildNumbers("", "");
    }

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