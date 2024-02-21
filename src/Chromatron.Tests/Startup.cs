// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Tests;

internal class Startup
{
    private static object _syncRoot = new();
    private static IServiceProvider? _provider;
    
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IChromatronDataTransferOptions, DataTransferOptions>();
        services.AddTransient<IChromatronModelBinder, DefaultModelBinder>();
        services.AddTransient<IChromatronRouteProvider, DefaultRouteProvider>();
        services.RegisterChromatronControllerAssembly(typeof(Startup).Assembly, ServiceLifetime.Singleton);
        services.AddDbContextFactory<TodoContext>();
    }

    public static IServiceProvider GetProvider()
    {
        if (_provider == null)
        {
            lock (_syncRoot)
            {
                if (_provider == null)
                {
                    var services = new ServiceCollection();
                    ConfigureServices(services);
                    _provider = services.BuildServiceProvider();
                }
            }
        }

        return _provider;
    }
}