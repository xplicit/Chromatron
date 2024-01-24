// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// Represents request scheme container.
/// </summary>
public interface IChromatronRequestSchemeProvider
{
    /// <summary>
    /// Adds scheme to the container.
    /// </summary>
    /// <param name="scheme"></param>
    void Add(UrlScheme scheme);

    /// <summary>
    /// Gets scheme from the container based on the request url.
    /// </summary>
    /// <param name="url">The request url./</param>
    /// <returns>Instance of <see cref="UrlScheme"/>.</returns>
    UrlScheme? GetScheme(string url);

    /// <summary>
    /// Get all schemes in the container.
    /// </summary>
    /// <returns>Collection of <see cref="UrlScheme"/> instances.</returns>
    List<UrlScheme> GetAllSchemes();

    /// <summary>
    /// Checks if a scheme is registered based on the url specified .
    /// </summary>
    /// <param name="url">The request url to check./</param>
    /// <returns>true if registered, otherwise false.</returns>
    bool IsSchemeRegistered(string url);
}