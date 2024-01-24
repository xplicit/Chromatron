// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Core.Infrastructure;

public static class RegisterAssembliesExtensions
{
    /// <summary>
    /// Register ChromatronController assembly.
    /// </summary>
    /// <remarks>
    /// The assembly (dll) are where atleast one custom controller class derived from <see cref="ChromatronController"/> is defined.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="assemby">The assembly where controller classes are defined.</param>
    /// <param name="lifetime">The services lifetime.</param>
    public static void RegisterChromatronControllerAssembly(this IServiceCollection services, Assembly assemby, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var types = from type in assemby.GetLoadableTypes()
                    where Attribute.IsDefined(type, typeof(ChromatronControllerAttribute))
                    select type;

        foreach (var type in types)
        {
            if (typeof(ChromatronController).IsAssignableFrom(type.BaseType))
            {
                services.Add(new ServiceDescriptor(typeof(ChromatronController), type, lifetime));
            }
        }
    }

    /// <summary>
    /// Register ChromatronController assemblies.
    /// </summary>
    /// <remarks>
    /// The assemblies (dlls) are where each has atleast one custom controller class derived from <see cref="ChromatronController"/> is defined.
    /// </remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="assemblies">The list of assemblies where controller classes are defined.</param>
    /// <param name="lifetime">The services lifetime.</param>
    public static void RegisterChromatronControllerAssemblies(this IServiceCollection services, Assembly[] assemblies, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        foreach (var assembly in assemblies)
        {
            RegisterChromatronControllerAssembly(services, assembly, lifetime);
        }
    }

    public static IEnumerable<Type?> GetLoadableTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t is not null);
        }
    }
}