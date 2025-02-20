﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Chromatron.NativeHosts.WinHost;

public static partial class Interop
{
    public static partial class User32
    {
        [DllImport(Libraries.User32, ExactSpelling = true)]
        internal static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public static int ReleaseDC(HandleRef hWnd, IntPtr hDC)
        {
            int result = ReleaseDC(hWnd.Handle, hDC);
            GC.KeepAlive(hWnd.Wrapper);
            return result;
        }

        public static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            int result = ReleaseDC(hWnd.Handle, hDC.Handle);
            GC.KeepAlive(hWnd.Wrapper);
            GC.KeepAlive(hDC.Wrapper);
            return result;
        }
    }
}
