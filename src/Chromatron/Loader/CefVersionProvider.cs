namespace Chromatron.Loader;

public static class CefVersionProvider
{
    /// <summary>The get expected chromium build number.</summary>
    /// <returns>The <see cref="int" />.</returns>
    public static CefBuildNumbers GetExpectedCefBuild()
    {
        try
        {
            return new CefBuildNumbers(CefRuntime.CefVersion, CefRuntime.ChromeVersion);
        }
        catch (Exception ex)
        {
            Logger.Instance.Log.LogError("Could not get expected chromium build number: {ex.Message}", ex.Message);
        }
        return new CefBuildNumbers(string.Empty, string.Empty);
    }

}