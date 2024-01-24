// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core;

/// <summary>
/// Chromatron information provider class.
/// </summary>
public interface IChromatronInfo
{
    /// <summary>
    /// Get the information data.
    /// </summary>
    /// <remarks>
    /// Info includes "About Chromatron", platform supported and the Chromium/CefGlue/CefSharp version.
    /// </remarks>
    /// <param name="requestId">Request identifier.</param>
    /// <returns>instance of <see cref="IChromatronResponse"/>.</returns>
    IChromatronResponse GetInfo(string requestId);

    /// <summary>
    /// Get the information data.
    /// </summary>
    /// <remarks>
    /// Info includes "About Chromatron", platform supported and the Chromium/CefGlue/CefSharp version.
    /// </remarks>
    /// <returns>Map of property and information data.</returns>
    IDictionary<string, string> GetInfo();
}