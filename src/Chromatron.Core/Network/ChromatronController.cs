﻿// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Network;

/// <summary>
/// Chromatron base controller class.
/// </summary>
public abstract class ChromatronController
{
    private string? _routePath;
    private string? _name;
    private string? _description;

    /// <summary>
    /// Gets the controller identifier name
    /// </summary>
    public string? Name
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                SetAttributeInfo();
            }

            return _name;
        }
    }

    /// <summary>
    /// Gets the controller route path.
    /// </summary>
    public string? RoutePath
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_routePath))
            {
                SetAttributeInfo();
            }

            return _routePath;
        }
    }

    /// <summary>
    /// Gets the controller description.
    /// </summary>
    public string? Description
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_description))
            {
                SetAttributeInfo();
            }

            return _description;
        }
    }

    private void SetAttributeInfo()
    {
        try
        {
            var attribute = GetType().GetCustomAttribute<ChromatronControllerAttribute>(true);
            if (attribute is not null)
            {
                _routePath = attribute.RoutePath;
                _name = attribute.Name;
                _description = attribute.Description;
            }
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception);
        }
    }
}