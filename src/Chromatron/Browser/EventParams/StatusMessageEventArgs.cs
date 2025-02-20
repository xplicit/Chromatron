﻿// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// The status message event args.
/// </summary>
public class StatusMessageEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StatusMessageEventArgs"/> class.
    /// </summary>
    /// <param name="value">
    /// The value.
    /// </param>
    public StatusMessageEventArgs(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; }
}