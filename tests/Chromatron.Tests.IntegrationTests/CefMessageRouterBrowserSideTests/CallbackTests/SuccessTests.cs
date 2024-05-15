using System.Net;
using System.Threading.Tasks;
using FluentAssertions;

namespace Chromatron.Tests.IntegrationTests.CefMessageRouterBrowserSideTests.CallbackTests
{
    public class SuccessTests : CallbackTestsBase
    {
        [Test]
        public async Task RequestInfo_ShouldBeOK()
        {
            // Arrange.
            AppendScriptWithQueryToIndex(request: "/info");

            // Act.
            var (completed, actual) = await RunAppForTestAndWaitForExit();

            // Assert.
            actual.Should().Be((int)HttpStatusCode.OK);
            completed.Should().BeTrue();
        }
    }
}
