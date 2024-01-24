// Copyright © 2024 Greeana LLC. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

namespace Chromatron.NativeHosts.MacHost;

internal class InteropMac
{
    internal const string ChromatronMacLib = "libchromatron.dylib";

    [StructLayout(LayoutKind.Sequential)]
    internal struct ChromatronParam
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public int centerscreen;
        public int frameless;
        public int fullscreen;
        public int noresize;
        public int nominbutton;
        public int nomaxbutton;
        public IntPtr titleUtf8Ptr;
        public IntPtr runMessageLoopCallback;
        public IntPtr cefShutdownCallback;
        public IntPtr initCallback;
        public IntPtr createCallback;
        public IntPtr movingCallback;
        public IntPtr resizeCallback;
        public IntPtr closeBrowserCallback;
        public IntPtr exitCallback;
    }

    [DllImport(ChromatronMacLib, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void createwindow(ref ChromatronParam chromatronParam);

    [DllImport(ChromatronMacLib, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void run(IntPtr application);

    [DllImport(ChromatronMacLib, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void quit(IntPtr application, IntPtr pool);

    [DllImport(ChromatronMacLib, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void minimize(IntPtr view);

    [DllImport(ChromatronMacLib, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void maximize(IntPtr view);
}
