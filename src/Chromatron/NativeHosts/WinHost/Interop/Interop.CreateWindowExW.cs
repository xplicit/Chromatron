﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable
#pragma warning disable CS0618
#pragma warning disable CA1401

namespace Chromatron.NativeHosts.WinHost;

public static partial class Interop
{
    public static partial class User32
    {
        [DllImport(Libraries.User32, CharSet = CharSet.Unicode, SetLastError = true)]
        internal unsafe static extern IntPtr CreateWindowExW(
            WS_EX dwExStyle,
            char* lpClassName,
            string lpWindowName,
            WS dwStyle,
            int X,
            int Y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInst,
            [MarshalAs(UnmanagedType.AsAny)] object lpParam);

        public unsafe static IntPtr CreateWindowExW(
            WS_EX dwExStyle,
            string lpClassName,
            string lpWindowName,
            WS dwStyle,
            int X,
            int Y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInst,
            object lpParam)
        {
            fixed (char* c = lpClassName)
            {
                return CreateWindowExW(dwExStyle, c, lpWindowName, dwStyle, X, Y, nWidth, nHeight, hWndParent, hMenu, hInst, lpParam);
            }
        }
    }
}