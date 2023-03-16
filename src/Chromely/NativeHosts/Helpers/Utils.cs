namespace Chromely.NativeHosts.Helpers;

public static class Utils
{
    public static void AssertNotNull(string methodName, IntPtr handle)
    {
        if (handle == IntPtr.Zero)
        {
            throw new Exception($"Handle not valid in Method:{methodName}");
        }
    }
}
