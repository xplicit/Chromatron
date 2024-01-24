// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core;

/// <summary>
/// This is used to create custom <see cref="IServiceProvider"/>.
/// </summary>
public abstract class ChromatronServiceProviderFactory
{
    /// <summary>
    /// Creates a <see cref="ServiceProvider"/> containing services from the provided <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>
    /// Note: 
    ///   1. This is where external dependency providers can be used to create<see cref="ServiceProvider"/>.
    ///   2. This is redundant for Owin based applications.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <returns>The <see cref="ServiceProvider"/> object.</returns>
    public abstract IServiceProvider BuildServiceProvider(IServiceCollection services);
}