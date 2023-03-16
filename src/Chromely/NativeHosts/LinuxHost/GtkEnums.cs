namespace Chromely.NativeHosts.LinuxHost;

[Flags]
public enum GtkDialogFlags
{
    Modal = 1 << 0,
    DestroyWithParent = 1 << 1,
}

public enum GtkMessageType
{
    Info,
    Warning,
    Question,
    Error,
    Other,
}

public enum GtkButtonsType
{
    None,
    Ok,
    Close,
    Cancel,
    YesNo,
    OkCancel,
}
