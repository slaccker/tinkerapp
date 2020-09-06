using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;

using Firebase.Database;
using Microsoft.Extensions.Caching.Memory;

using Web.Models;
using Web.NewsSources;
using Web.Interfaces;
using Web.NewsSources.HackerNews;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace UnitTests.NewsSourcesTests.HackerNewsTests.HackerNewsDataSourceTests
{
    /// <summary>
    /// Test HackerNews implementation of INewsSourceDataSource
    /// </summary>
    public class INewsSourceDataSourceTests
    {
        #region Fields

        private NewsSourceHackerNews _newsSourceHackerNews;

        private INewsSourceDataSource<FirebaseClient> _dataSource;

        private readonly string _expectedApiURL = "https://hacker-news.firebaseio.com/v0";

        private int _startAtOffset = -1;
        private int _perPageLimit = 5;

        protected Mock<ILogger<NewsSourceHackerNews>> _loggerMock;

        protected readonly ITestOutputHelper _testOutputHelper;

        protected readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions() { SizeLimit = 500 });

        #endregion

        public INewsSourceDataSourceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            _loggerMock = new Mock<ILogger<NewsSourceHackerNews>>();

            Mock<ILogger<HackerNewsDataSource>> hndsLoggerMock = new Mock<ILogger<HackerNewsDataSource>>();
            _dataSource = new HackerNewsDataSource(hndsLoggerMock.Object);

            _newsSourceHackerNews = new NewsSourceHackerNews(_loggerMock.Object, _dataSource, _cache);
            _newsSourceHackerNews.SetDataSourceBaseURL(_expectedApiURL);

        }

        #region ConfirmApiBehaviour

        [Fact]
        public async Task Test_GetMaxItemIDAsync_ApiTest__Async()
        {
            // Arrange

            // Act
            int result = await _newsSourceHackerNews.GetMaxItemIDAsync();
            _testOutputHelper.WriteLine($"MaxItemID returned: {result} <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

            // Assert
            result.Should().BePositive();
        }

        [Fact]
        public async Task Test_GetStoryKeysAsync_ApiTest__Async()
        {
            // Arrange

            // Act
            var results = await _newsSourceHackerNews.GetStoryKeysAsync();
            _testOutputHelper.WriteLine("StoryKeys returned: ", results.GetEnumerator().Current, " <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

            // Assert
            results.Should().BeOfType<List<int>>();
        }

        [Fact]
        public async Task Test_GetNewsStoryListAsync_ApiTest__Async()
        {
            // Arrange

            // Act
            var results = await _newsSourceHackerNews.GetNewsStoryListAsync(_startAtOffset, _perPageLimit);
            _testOutputHelper.WriteLine("NewsStoryList returned: ", results.GetEnumerator().Current, " <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

            // Assert
            results.Should().BeOfType<List<NewsStory>>();
        }

        #endregion
    }
}