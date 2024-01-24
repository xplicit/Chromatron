// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.Browser;

/// <summary>
/// Default CEF render process handler.
/// </summary>
public class DefaultRenderProcessHandler : CefRenderProcessHandler
{
    protected readonly IChromatronConfiguration _config;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultRenderProcessHandler"/> class.
    /// </summary>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    public DefaultRenderProcessHandler(IChromatronConfiguration config)
    {
        _config = config;
        MessageRouter = new CefMessageRouterRendererSide(new CefMessageRouterConfig());
    }

    /// <summary>
    /// Gets the message router.
    /// </summary>
    public CefMessageRouterRendererSide MessageRouter { get; }

    /// <inheritdoc/>
    protected override void OnContextCreated(CefBrowser browser, CefFrame frame, CefV8Context context)
    {
        MessageRouter.OnContextCreated(browser, frame, context);

        // MessageRouter.OnContextCreated doesn't capture CefV8Context immediately,
        // so we able to release it immediately in this call.
        context.Dispose();
    }

    /// <inheritdoc/>
    protected override void OnContextReleased(CefBrowser browser, CefFrame frame, CefV8Context context)
    {
        // MessageRouter.OnContextReleased releases captured CefV8Context (if have).
        MessageRouter.OnContextReleased(browser, frame, context);

        // Release CefV8Context.
        context.Dispose();
    }

    /// <inheritdoc/>
    protected override bool OnProcessMessageReceived(CefBrowser browser, CefFrame frame, CefProcessId sourceProcess, CefProcessMessage message)
    {
        var handled = MessageRouter.OnProcessMessageReceived(browser, sourceProcess, message);
        if (handled)
        {
            return true;
        }

        return false;
    }
}