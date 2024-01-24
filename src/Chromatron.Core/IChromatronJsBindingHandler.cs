// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core;

/// <summary>
/// Represents JavaScript binding handler.
/// </summary>
public interface IChromatronJsBindingHandler
{
    /// <summary>
    /// Gets the identifier key.
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Gets or sets the binding object name.
    /// </summary>
    string ObjectName { get; set; }

    /// <summary>
    /// Gets or sets the binding object.
    /// </summary>
    object? BoundObject { get; set; }

    /// <summary>
    /// Gets or sets the binding options.
    /// </summary>
    object? BindingOptions { get; set; }
}