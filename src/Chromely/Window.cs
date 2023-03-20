// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromely;

/// <summary>
/// Main browser window.
/// </summary>
public partial class Window : ChromiumBrowser, IChromelyWindow
{
    /// <summary>
    /// Initializes a new instance of <see cref="Window"/>.
    /// </summary>
    /// <param name="nativeHost">The native host [Windows - win32, Linux - Gtk, MacOS - Cocoa] of type <see cref="IChromelyNativeHost"/>.</param>
    /// <param name="config">Instance of <see cref="IChromelyConfiguration"/>.</param>
    /// <param name="handlersResolver">instance of <see cref="ChromelyHandlersResolver"/>.</param>
    public Window(IChromelyNativeHost nativeHost,
                  IChromelyConfiguration config,
                  ChromelyHandlersResolver handlersResolver)
        : base(nativeHost, config, handlersResolver)
    {
        Created += OnBrowserCreated;
    }

    /// <inheritdoc/>
    public IntPtr Handle
    {
        get
        {
            if (NativeHost is not null)
            {
                return NativeHost.Handle;
            }

            return IntPtr.Zero;
        }
    }

    /// <inheritdoc/>
    public virtual void Init(object settings)
    {
    }

    /// <inheritdoc/>
    public virtual void Create(IntPtr hostHandle, IntPtr winXID)
    {
        CreateBrowser(hostHandle, winXID);
    }

    /// <inheritdoc/>
    public virtual void SetTitle(string title)
    {
        NativeHost?.SetWindowTitle(title);
    }

    /// <inheritdoc/>
    public virtual void NotifyOnMove()
    {
        NotifyMoveOrResize();
    }

    /// <inheritdoc/>
    public virtual void Resize(int width, int height)
    {
        ResizeBrowser(width, height);
    }

    /// <inheritdoc/>
    public virtual void Minimize()
    {
        NativeHost?.SetWindowState(WindowState.Minimize);
    }

    /// <inheritdoc/>
    public virtual void Maximize()
    {
        NativeHost?.SetWindowState(WindowState.Maximize);
    }

    /// <inheritdoc/>
    public virtual void Restore()
    {
        NativeHost?.SetWindowState(WindowState.Normal);
    }

    /// <inheritdoc/>
    public virtual void Close()
    {
        NativeHost?.Shutdown();
    }
}
