// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#nullable disable

namespace Chromatron.Browser;

internal interface IDefaultCustomHandler
{
}

internal interface IDefaultResourceCustomHandler
{
}


internal interface IDefaultAssemblyResourceCustomHandler
{
}

internal interface IDefaultRequestCustomHandler
{
}

internal interface IDefaultExernalRequestCustomHandler
{
}

/*
 * Message Router
 */
internal sealed class ChromatronMessageRouter : DefaultMessageRouterHandler
{
    public ChromatronMessageRouter(IChromatronRouteProvider routeProvider, IChromatronRequestHandler requestHandler, IChromatronDataTransferOptions dataTransferOptions, IChromatronErrorHandler chromatronErrorHandler)
        : base(routeProvider, requestHandler, dataTransferOptions, chromatronErrorHandler)
    {
    }
}

/*
 * Resource/Request Scheme handlers
 */
internal sealed class ChromatronResourceSchemeHandlerFactory : DefaultResourceSchemeHandlerFactory, IDefaultResourceCustomHandler
{
    public ChromatronResourceSchemeHandlerFactory(IChromatronConfiguration config, IChromatronErrorHandler chromatronErrorHandler) : base(config, chromatronErrorHandler)
    {
    }
}

internal sealed class ChromatronAssemblyResourceSchemeHandlerFactory : DefaultAssemblyResourceSchemeHandlerFactory, IDefaultAssemblyResourceCustomHandler
{
    public ChromatronAssemblyResourceSchemeHandlerFactory(IChromatronConfiguration config, IChromatronErrorHandler chromatronErrorHandler) : base(config, chromatronErrorHandler)
    {
    }
}

internal sealed class ChromatronRequestSchemeHandlerFactory : DefaultRequestSchemeHandlerFactory, IDefaultRequestCustomHandler
{
    public ChromatronRequestSchemeHandlerFactory(IChromatronRouteProvider routeProvider, IChromatronRequestSchemeProvider requestSchemeProvider, IChromatronRequestHandler requestHandler, IChromatronDataTransferOptions dataTransferOptions, IChromatronErrorHandler chromatronErrorHandler)
        : base(routeProvider, requestSchemeProvider, requestHandler, dataTransferOptions, chromatronErrorHandler)
    {
    }
}

internal sealed class ChromatronExternalRequestSchemeHandlerFactory : DefaultExternalRequestSchemeHandlerFactory, IDefaultExernalRequestCustomHandler
{
}

/*
 * Custom handlers
 */
internal sealed class ChromatronContextMenuHandler : DefaultContextMenuHandler, IDefaultCustomHandler
{
    public ChromatronContextMenuHandler(IChromatronConfiguration config) : base(config)
    {
    }
}

internal sealed class ChromatronDisplayHandler : DefaultDisplayHandler, IDefaultCustomHandler
{
    public ChromatronDisplayHandler(IChromatronConfiguration config, IChromatronWindow window) : base(config, window)
    {
    }
}

internal sealed class ChromatronLoadHandler : DefaultLoadHandler, IDefaultCustomHandler
{
    public ChromatronLoadHandler(IChromatronConfiguration config, IChromatronWindow window) : base(config, window)
    {
    }
}

internal sealed class ChromatronDownloadHandler : DefaultDownloadHandler, IDefaultCustomHandler
{
}

internal sealed class ChromatronDragHandler : DefaultDragHandler, IDefaultCustomHandler
{
    public ChromatronDragHandler(IChromatronConfiguration config) : base(config)
    {
    }
}

internal sealed class ChromatronLifeSpanHandler : DefaultLifeSpanHandler, IDefaultCustomHandler
{
    public ChromatronLifeSpanHandler(IChromatronConfiguration config, IChromatronRequestHandler requestHandler, IChromatronRouteProvider routeProvider, IChromatronWindow window)
        : base(config, requestHandler, routeProvider, window)
    {
    }
}

internal sealed class ChromatronRequestHandler : DefaultRequestHandler, IDefaultCustomHandler
{
    public ChromatronRequestHandler(IChromatronConfiguration config, IChromatronRequestHandler requestHandler, IChromatronRouteProvider routeProvider, IChromatronWindow window, CefResourceRequestHandler resourceRequestHandler = null)
            : base(config, requestHandler, routeProvider, window, resourceRequestHandler)
    {
    }
}

public interface IDefaultOwinCustomHandler
{
}