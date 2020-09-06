using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Moq;

using Firebase.Database;
using Microsoft.Extensions.Caching.Memory;

using Web.NewsSources;
using Web.Interfaces;

namespace UnitTests.NewsSourcesTests.NewsSourceHackerNewsTests
{
    public class TestBase
    {
        #region Fields

        protected NewsSourceHackerNews _newsSourceHackerNews;
        protected Mock<INewsSourceDataSource<FirebaseClient>> _datasourceMock;

        protected readonly string _mockURL = "https://mock.mock";

        protected ILogger<NewsSourceHackerNews> _logger; //NullLogger<NewsSourceHackerNews> _logger;

        protected Mock<ILogger<NewsSourceHackerNews>> _loggerMock;

        protected readonly ITestOutputHelper _testOutputHelper;

        protected readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 500 });

        #endregion

        public TestBase(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            _loggerMock = new Mock<ILogger<NewsSourceHackerNews>>();
            _loggerMock.VerifyAll();

            _logger = _loggerMock.Object; //new NullLogger<NewsSourceHackerNews>();

            var firebaseClientMock = new Mock<FirebaseClient>(MockBehavior.Strict, new object[] { _mockURL, null });
            _datasourceMock = new Mock<INewsSourceDataSource<FirebaseClient>>();
            _datasourceMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(firebaseClientMock.Object);
            _datasourceMock.Setup(x => x.CreateClient(It.IsAny<string>())).Verifiable();
            _datasourceMock.Setup(x => x.GetClient()).Returns(firebaseClientMock.Object);
            _datasourceMock.Setup(x => x.GetClient()).Verifiable();

            _newsSourceHackerNews = new NewsSourceHackerNews(_logger, _datasourceMock.Object, _cache);
        }
    }
}