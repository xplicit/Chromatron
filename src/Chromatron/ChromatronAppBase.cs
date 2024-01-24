// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

using Chromatron.NativeHosts.LinuxHost;
using Chromatron.NativeHosts.MacHost;
using Chromatron.NativeHosts.WinHost;
using Chromatron.NativeHosts.WinHost.Hooks;
using Chromatron.NativeHosts.WinHost.WinBase;

namespace Chromatron;

/// <inheritdoc/>
public abstract class ChromatronAppBase : ChromatronApp
{
    /// <inheritdoc/>
    public override void ConfigureCoreServices(IServiceCollection services)
    {
        base.ConfigureCoreServices(services);

        // Add window core services if not already added.

        services.TryAddSingleton<IChromatronInfo, ChromatronInfo>();
        services.TryAddSingleton<IChromatronRouteProvider, DefaultRouteProvider>();
        services.TryAddSingleton<IChromatronDataTransferOptions, DataTransferOptions>();
        services.TryAddSingleton<IChromatronModelBinder, DefaultModelBinder>();
        services.TryAddSingleton<IChromatronRequestHandler, DefaultActionRequestHandler>();
        services.TryAddSingleton<ICefDownloader, DefaultCefDownloader>();

        services.TryAddSingleton<IChromatronWindow, Window>();
        services.TryAddSingleton<ChromatronWindowController, WindowController>();

        var platform = ChromatronRuntime.Platform;

        switch (platform)
        {
            case ChromatronPlatform.MacOSX:
                services.TryAddSingleton<IChromatronNativeHost, ChromatronMacHost>();
                break;

            case ChromatronPlatform.Linux:
                services.TryAddSingleton<IChromatronNativeHost, ChromatronLinuxHost>();
                break;

            case ChromatronPlatform.Windows:
                services.TryAddSingleton<IWindowMessageInterceptor, DefaultWindowMessageInterceptor>();
                services.TryAddSingleton<IKeyboardHookHandler, DefaultKeyboardHookHandler>();
                services.TryAddSingleton<IChromatronNativeHost, ChromatronWinHost>();
                break;

            default:
                services.TryAddSingleton<IWindowMessageInterceptor, DefaultWindowMessageInterceptor>();
                services.TryAddSingleton<IKeyboardHookHandler, DefaultKeyboardHookHandler>();
                services.TryAddSingleton<IChromatronNativeHost, ChromatronWinHost>();
                break;
        }
    }

    /// <inheritdoc/>
    public override void ConfigureDefaultHandlers(IServiceCollection services)
    {
        base.ConfigureDefaultHandlers(services);

        services.AddSingleton<IChromatronMessageRouter, ChromatronMessageRouter>();

        // Add default resource/request handlers
        services.AddSingleton<IChromatronRequestSchemeProvider, DefaultRequestSchemeProvider>();

        services.AddSingleton<CefSchemeHandlerFactory, ChromatronResourceSchemeHandlerFactory>();
        services.AddSingleton<CefSchemeHandlerFactory, ChromatronAssemblyResourceSchemeHandlerFactory>();
        services.AddSingleton<CefSchemeHandlerFactory, ChromatronRequestSchemeHandlerFactory>();
        services.AddSingleton<CefSchemeHandlerFactory, ChromatronExternalRequestSchemeHandlerFactory>();

        // Adde default custom handlers
        services.AddSingleton<CefContextMenuHandler, ChromatronContextMenuHandler>();
        services.AddSingleton<CefDisplayHandler, ChromatronDisplayHandler>();
        services.AddSingleton<CefDownloadHandler, ChromatronDownloadHandler>();
        services.AddSingleton<CefDragHandler, ChromatronDragHandler>();
        services.AddSingleton<CefLifeSpanHandler, ChromatronLifeSpanHandler>();
        services.AddSingleton<CefLoadHandler, ChromatronLoadHandler>();
        services.AddSingleton<CefRequestHandler, ChromatronRequestHandler>();
    }
}