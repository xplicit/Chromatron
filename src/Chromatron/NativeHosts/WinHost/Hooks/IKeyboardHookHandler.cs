// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.NativeHosts.WinHost.Hooks;

/// <summary>
/// Represents Windows OS keyboard handler.
/// </summary>
public interface IKeyboardHookHandler
{
    /// <summary>
    /// Set the Windows OS native host.
    /// </summary>
    /// <param name="nativeHost">The Windows native host of type <see cref="IChromatronNativeHost"/>.</param>
    void SetNativeHost(IChromatronNativeHost nativeHost);

    /// <summary>
    /// Handler keyboard keys.
    /// </summary>
    /// <param name="handle">Windows OS handle.</param>
    /// <param name="param">The key parameter.</param>
    /// <returns>true is handled, otherwise false.</returns>
    bool HandleKey(IntPtr handle, object param);
}