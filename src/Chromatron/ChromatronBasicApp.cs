// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron;

/// <summary>
/// Simplest Chromatron app implementation.
/// Be sure to call base implementations on derived implementations.
/// </summary>
public class ChromatronBasicApp : ChromatronAppBase
{
    /// <inheritdoc/>
    public sealed override void ConfigureCoreServices(IServiceCollection services)
    {
        base.ConfigureCoreServices(services);
    }

    /// <inheritdoc/>
    public sealed override void ConfigureDefaultHandlers(IServiceCollection services)
    {
        base.ConfigureDefaultHandlers(services);
    }
}