namespace Chromely.NativeHosts.Helpers;

internal static class IconHandler
{
    public static IntPtr? LoadIconFromFile(string iconFullPath)
    {
        if (string.IsNullOrEmpty(iconFullPath))
        {
            return null;
        }

        if (!File.Exists(iconFullPath))
        {
            // If local file
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            iconFullPath = Path.Combine(appDirectory, iconFullPath);
            if (!File.Exists(iconFullPath))
            {
                return null;
            }
        }

        return LoadImage( // returns a HANDLE so we have to cast to HICON
            IntPtr.Zero, // hInstance must be NULL when loading from a file
            iconFullPath, // the icon file name
            (uint)LR.IMAGE_ICON, // specifies that the file is an icon
            0, // width of the image (we'll specify default later on)
            0, // height of the image
            (uint)LR.LOADFROMFILE | // we want to load a file (as opposed to a resource)
            (uint)LR.DEFAULTSIZE | // default metrics based on the type (IMAGE_ICON, 32x32)
            (uint)LR.SHARED // let the system release the handle when it's no longer used
        );
    }
    
    public static string IconFullPath(string iconFile)
    {
        try
        {
            if (string.IsNullOrEmpty(iconFile))
            {
                return iconFile;
            }

            if (!File.Exists(iconFile) || !Path.IsPathRooted(iconFile))
            {
                // If local file
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                iconFile = Path.Combine(appDirectory, iconFile);
                if (!File.Exists(iconFile))
                {
                    return string.Empty;
                }

                return iconFile;
            }

            return iconFile;
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception, "IconHandler:IconFullPath");
        }

        return iconFile;
    }

    public static IntPtr IconFileToPtr(string iconFile)
    {
        try
        {
            iconFile = IconFullPath(iconFile);
            if (string.IsNullOrEmpty(iconFile))
            {
                return IntPtr.Zero;
            }

            var iconBytes = File.ReadAllBytes(iconFile);
            return Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(byte)) * iconBytes.Length);
        }
        catch (Exception exception)
        {
            Logger.Instance.Log.LogError(exception, "IconHandler:IconFileToPtr");
        }

        return IntPtr.Zero;
    }
}
