// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Tests;

internal class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IChromatronDataTransferOptions, DataTransferOptions>();
        services.AddTransient<IChromatronModelBinder, DefaultModelBinder>();
        services.AddTransient<IChromatronRouteProvider, DefaultRouteProvider>();
        services.RegisterChromatronControllerAssembly(typeof(Startup).Assembly, ServiceLifetime.Singleton);
        services.AddDbContextFactory<TodoContext>();
    }
}