﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Chromatron.NativeHosts.WinHost;

public static partial class Interop
{
    public static partial class User32
    {
        public struct DRAWITEMSTRUCT
        {
            public ODT CtlType;
            public uint CtlID;
            public uint itemID;
            public ODA itemAction;
            public ODS itemState;
            public IntPtr hwndItem;
            public IntPtr hDC;
            public RECT rcItem;
            public IntPtr itemData;
        }
    }
}