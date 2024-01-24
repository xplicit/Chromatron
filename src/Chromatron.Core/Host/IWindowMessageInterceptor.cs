// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Host;

/// <summary>
/// Represents window messsage loop interceptor.
/// </summary>
public interface IWindowMessageInterceptor
{
    /// <summary>
    /// This sets up the service.
    /// </summary>
    /// <param name="nativeHost">Chromatron native host - instance of <see cref="IChromatronNativeHost"/>.</param>
    /// <param name="browserHandle">The browser window handle.</param>
    void Setup(IChromatronNativeHost nativeHost, IntPtr browserHandle);
}