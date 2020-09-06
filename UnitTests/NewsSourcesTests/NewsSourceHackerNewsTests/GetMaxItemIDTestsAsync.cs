using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using System.Threading.Tasks;
using Moq;

using Firebase.Database;

using Web.NewsSources;
using Web.Interfaces;
using Web.NewsSources.HackerNews;

namespace UnitTests.NewsSourcesTests.NewsSourceHackerNewsTests
{
    public class GetMaxItemIDAsyncTests : TestBase

    {
        private readonly int _newestStoryID = 123456;

        public GetMaxItemIDAsyncTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            var firebaseClientMock = new Mock<FirebaseClient>(MockBehavior.Strict, new object[] { _mockURL, null });
            _datasourceMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(firebaseClientMock.Object);
            _datasourceMock.Setup(x => x.CreateClient(It.IsAny<string>())).Verifiable();
            _datasourceMock.Setup(x => x.GetClient()).Returns(firebaseClientMock.Object);
            _datasourceMock.Setup(x => x.GetClient()).Verifiable();
            _datasourceMock.Setup(x => x.GetNewestStoryID()).ReturnsAsync(_newestStoryID);
        }

        [Fact]
        public async Task Test_MaxItemIDAsync_ReturnsExpected_Async()
        {
            // Arrange
            _newsSourceHackerNews.SetDataSourceBaseURL(_mockURL);

            int result = await _newsSourceHackerNews.GetMaxItemIDAsync();
            _testOutputHelper.WriteLine($"MaxItemID returned: {result}");

            result.Should().Be(_newestStoryID);
        }

    }
}
