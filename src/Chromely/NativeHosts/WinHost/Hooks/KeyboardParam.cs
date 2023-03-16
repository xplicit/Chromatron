using Keys = Chromely.NativeHosts.WinHost.Interop.User32.Keys;

namespace Chromely.NativeHosts.WinHost.Hooks;

internal class KeyboardParam
{
    public KeyboardParam(bool isKeyUp, bool alt, bool control, Keys key)
    {
        IsKeyUp = isKeyUp;
        Alt = alt;
        Control = control;
        Key = key;
    }
    public bool IsKeyUp { get; set; }
    public bool Alt { get; set; }
    public bool Control { get; set; }
    public Keys Key { get; set; }
}
