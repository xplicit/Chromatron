using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Chromatron.Tests.IntegrationTests.CefMessageRouterBrowserSideTests.CallbackTests
{
    public class CallbackTestsBase
    {
        private readonly TimeSpan defaultTimeoutToExecute = TimeSpan.FromMinutes(5);
        private readonly ProcessStartInfo appForTestsProcessInfo = new("AppForTests.exe")
        {
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden,
        };

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Save the original, because it will be changed in the test
            File.Copy("index.html", "index.backup", overwrite: true);
        }

        [TearDown]
        public void TearDown()
        {
            // Restore the original
            File.Copy("index.backup", "index.html", overwrite: true);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Delete the backup after all tests
            File.Delete("index.backup");
        }

        protected void AppendScriptWithQueryToIndex(string request)
        {
            var script = @"
window.cefQuery({
    request: JSON.stringify({
        ""url"": """ + request + @""",
        ""parameters"": null,
        ""postData"": null,
    }),

    onSuccess: function (responseJson) {
        console.info(responseJson);
    },

    onFailure: function (err, msg) {
    }
});
";

            var html = File.ReadAllText("index.html");
            html = html.Replace("<script>", $"<script>{script}");
            File.WriteAllText("index.html", html);
        }

        protected Task<(bool completed, int exitCode)> RunAppForTestAndWaitForExit() =>
            RunAppForTestAndWaitForExit(defaultTimeoutToExecute);

        protected Task<(bool completed, int exitCode)> RunAppForTestAndWaitForExit(TimeSpan timeoutToExecute)
        {
            return Task.Run(() =>
            {
                using (var appForTests = Process.Start(appForTestsProcessInfo))
                {
                    var completed = appForTests?.WaitForExit(timeoutToExecute) ?? false;
                    return (completed, completed ? appForTests.ExitCode : -1);
                }
            });
        }
    }
}
