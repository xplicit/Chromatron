﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Chromatron.NativeHosts.WinHost;

public static partial class Interop
{
    public static partial class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public BOOL fErase;
            public RECT rcPaint;
            public BOOL fRestore;
            public BOOL fIncUpdate;
            public fixed byte rgbReserved[32];
        }
    }
}