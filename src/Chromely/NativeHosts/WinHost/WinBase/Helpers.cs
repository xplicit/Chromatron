// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

#pragma warning disable CA1822
#pragma warning disable CA2211

namespace Chromely.NativeHosts.WinHost.WinBase;

public class Win32WindowStyles
{
    public static WS NormalStyles = WS.OVERLAPPEDWINDOW | WS.CLIPCHILDREN | WS.CLIPSIBLINGS;
    public static WS_EX NormalExStyles = WS_EX.APPWINDOW | WS_EX.WINDOWEDGE;

    public Win32WindowStyles(IWindowOptions options)
    {
        RECT = new Rectangle(options.Position.X, options.Position.Y, options.Size.Width, options.Size.Height);
        WindowPlacement = new WINDOWPLACEMENT();
        if (options.CustomStyle is not null &&
            options.CustomStyle.WindowStyles != 0 &&
            options.CustomStyle.WindowExStyles != 0)
        {
            Styles = (WS)options.CustomStyle.WindowStyles;
            ExStyles = (WS_EX)options.CustomStyle.WindowExStyles;
        }
        else
        {
            Styles = NormalStyles;
            ExStyles = NormalExStyles;
        }

        State = options.WindowState;
    }

    public WindowState State { get; set; }
    public WS Styles { get; set; }
    public WS_EX ExStyles { get; set; }
    public RECT RECT { get; set; }
    public SW ShowCommand { get; set; }
    public WINDOWPLACEMENT WindowPlacement { get; set; }
    public WS FullscreenStyles
    {
        get
        {
            var styles = WS.OVERLAPPEDWINDOW | WS.CLIPCHILDREN | WS.CLIPSIBLINGS;
            styles &= ~(WS.CAPTION);
            return styles;
        }
    }
    public WS_EX FullscreenExStyles
    {
        get
        {
            var exStyles = WS_EX.APPWINDOW;
            exStyles &= ~(WS_EX.DLGMODALFRAME | WS_EX.WINDOWEDGE | WS_EX.CLIENTEDGE | WS_EX.STATICEDGE);
            return exStyles;
        }
    }
}

internal static class WindowHelper
{
    public static class HwndZOrder
    {
        public static IntPtr
        NoTopMost = new(-2),
        TopMost = new(-1),
        Top = new(0),
        Bottom = new(1);
    }

    public static void CenterWindowToScreen(IntPtr hwnd, bool useWorkArea = true)
    {
        try
        {
            IntPtr handle = MonitorFromWindow(GetDesktopWindow(), MONITOR.DEFAULTTONEAREST);

            MONITORINFOEXW monInfo = new(null);
            monInfo.cbSize = (uint)Marshal.SizeOf(monInfo);

            GetMonitorInfoW(handle, ref monInfo);
            var screenRect = useWorkArea ? monInfo.rcWork : monInfo.rcMonitor;
            var midX = (monInfo.rcMonitor.right - monInfo.rcMonitor.left) / 2;
            var midY = (monInfo.rcMonitor.bottom - monInfo.rcMonitor.top) / 2;
            var size = GetWindowSize(hwnd);
            var left = midX - (size.Width / 2);
            var top = midY - (size.Height / 2);

            SetWindowPos(
                hwnd,
                IntPtr.Zero,
                left,
                top,
                -1,
                -1,
                SWP.NOACTIVATE | SWP.NOSIZE | SWP.NOZORDER);
        }
        catch { }
        {
        }
    }

    public static Rectangle FullScreenBounds(Rectangle configuredBounds)
    {
        try
        {
            IntPtr handle = MonitorFromWindow(GetDesktopWindow(), MONITOR.DEFAULTTOPRIMARY);

            MONITORINFOEXW monInfo = new(null);

            GetMonitorInfoW(handle, ref monInfo);
            RECT rect = monInfo.rcMonitor;

            if (rect.Width <= 0 || rect.Height <= 0) return configuredBounds;

            return rect;
        }
        catch { }
        {
        }

        return configuredBounds;
    }

    public static Size GetWindowSize(IntPtr hwnd)
    {
        var size = new Size();
        if (hwnd != IntPtr.Zero)
        {
            RECT rect = new();
            GetWindowRect(hwnd, ref rect);
            size.Width = rect.Width;
            size.Height = rect.Height;
        }

        return size;
    }
}
