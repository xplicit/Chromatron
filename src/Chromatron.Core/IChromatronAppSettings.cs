// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core;

/// <summary>
/// Chromatron application setting.
/// </summary>
/// <remarks>
/// This is used for storing and retrieval of application/user settings.
/// </remarks>
public interface IChromatronAppSettings
{
    /// <summary>
    /// Gets or sets the application name.
    /// </summary>
    string AppName { get; set; }

    /// <summary>
    /// Gets the path for storage of the file used to store the data.
    /// </summary>
    string? DataPath { get; }

    /// <summary>
    /// Gets the settings object.
    /// </summary>
    /// <remarks>
    /// This is a dynamic object to allow for any type of data to be stored using a key/value pair format.
    /// </remarks>
    dynamic Settings { get; }

    /// <summary>
    /// Reads the configuration data from a previously stored configuration file.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    void Read(IChromatronConfiguration config);

    /// <summary>
    /// Saves the configuration data to a configuration file.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    void Save(IChromatronConfiguration config);
}