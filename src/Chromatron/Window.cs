// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron;

/// <summary>
/// Main browser window.
/// </summary>
public partial class Window : ChromiumBrowser, IChromatronWindow
{
    /// <summary>
    /// Initializes a new instance of <see cref="Window"/>.
    /// </summary>
    /// <param name="nativeHost">The native host [Windows - win32, Linux - Gtk, MacOS - Cocoa] of type <see cref="IChromatronNativeHost"/>.</param>
    /// <param name="config">Instance of <see cref="IChromatronConfiguration"/>.</param>
    /// <param name="handlersResolver">instance of <see cref="ChromatronHandlersResolver"/>.</param>
    public Window(IChromatronNativeHost nativeHost,
                  IChromatronConfiguration config,
                  ChromatronHandlersResolver handlersResolver)
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
