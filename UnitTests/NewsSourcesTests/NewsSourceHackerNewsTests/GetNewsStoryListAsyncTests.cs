using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Moq;

using Firebase.Database;

using Web.NewsSources;
using Web.Interfaces;
using Microsoft.Extensions.Logging;
using Web.NewsSources.HackerNews;
using Web.Models;

namespace UnitTests.NewsSourcesTests.NewsSourceHackerNewsTests
{
    public class GetNewsStoryListAsyncTests : TestBase
    {
        #region Fields

        private List<int> _testStoryKeysList = new List<int>();
        private List<HackerNewsItem> _testHackerNewsItemList = new List<HackerNewsItem>();

        #endregion

        public GetNewsStoryListAsyncTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _newsSourceHackerNews.SetDataSourceBaseURL(_mockURL);

            SetupDatasourceMock();
        }

        #region Mock Setup

        private void SetupDatasourceMock(int count = 15)
        {
            // StoryKeys
            _testStoryKeysList.Clear();
            for (var i = 1; i <= count; i++)
            {
                var genVal = "" + i + i + i;
                _testStoryKeysList.Add(int.Parse(genVal));
            }
            _datasourceMock.Setup(x => x.GetStoryKeysAsyncTypeNewest()).ReturnsAsync(_testStoryKeysList);

            // StoryDetail
            _testHackerNewsItemList.Clear();
            foreach (var key in _testStoryKeysList)
            {
                var url = $"http://story-{key}/mocknews.mock";
                if (key == 222)
                {
                    url = ""; // empty url
                }

                _testHackerNewsItemList.Add(new HackerNewsItem()
                {
                    id = key,
                    type = "story",
                    by = "testuser",
                    title = $"<b>test story {key} title</b>",
                    text = "<p>test story text</p><div>Something interesting?</div>",
                    url = url,

                });
            }
            int callModIdx = -1;
            _datasourceMock.Setup(x => x.GetStoryDetailAsync<HackerNewsItem>(It.IsAny<int>())).ReturnsAsync(() =>
            {
                /* Returns test data in sequential manner */

                // Return results at callModIdx
                callModIdx++;
                if (callModIdx % _testHackerNewsItemList.Count == 0) callModIdx = 0;

                return _testHackerNewsItemList[callModIdx];
            });
        }

        #endregion

        [Fact]
        public async Task Test_GetNewsStoryListAsync_ReturnListOfExpectedCount_Async()
        {
            // Arrange


            // Act
            var results = await _newsSourceHackerNews.GetNewsStoryListAsync();

            // Assert
            results.Should().BeOfType<List<NewsStory>>();
            results.Should().HaveCount(_testStoryKeysList.Count);
        }

        [Fact]
        public async Task Test_GetNewsStoryListAsync_HandlesEmptyStoryDetail_Async()
        {
            // Arrange
            SetupDatasourceMock();
            //   Remove some Story Details
            var removeModPosition = 6;
            var countStoryDetail = _testHackerNewsItemList.Count;
            var countRemovedStoryDetail = 0;
            for (var i = 0; i < countStoryDetail; i++)
            {
                if (i % removeModPosition == 0)
                {
                    _testHackerNewsItemList[i] = null;
                    countRemovedStoryDetail++;
                }
            }
            var expectedCount = _testHackerNewsItemList.Count - countRemovedStoryDetail;

            // Act
            var results = await _newsSourceHackerNews.GetNewsStoryListAsync();

            // Assert
            results.Should().BeOfType<List<NewsStory>>();
            results.Should().HaveCount(expectedCount);

            // Derived from https://stackoverflow.com/a/58413842
            _loggerMock.Verify(x =>
                x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().StartsWith("Story ")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((o, t) => true))
                , Times.Exactly(countRemovedStoryDetail)
            );
        }

        [Fact]
        public async Task Test_GetNewsStoryListAsync_StartAtReturnsExpectedIndex_Async()
        {
            // Arrange
            SetupDatasourceMock();
            var expectedStartAtIndex = 6;
            var expectedCount = _testStoryKeysList.Count - expectedStartAtIndex;

            // Act
            var results = await _newsSourceHackerNews.GetNewsStoryListAsync(expectedStartAtIndex);


            // Assert
            results.Should().BeOfType<List<NewsStory>>();
            results.Should().HaveCount(expectedCount);

            var resultsList = (List<NewsStory>)results;
            resultsList[0].Id.Should().Be(_testStoryKeysList[expectedStartAtIndex]);
        }

        [Fact]
        public async Task Test_GetNewsStoryListAsync_PageLimit_Async()
        {
            // Arrange
            SetupDatasourceMock();
            var expectedPageLimit = 5;
            var expectedStartAtIndex = 0;

            // Act
            var results = await _newsSourceHackerNews.GetNewsStoryListAsync(expectedStartAtIndex, expectedPageLimit);


            // Assert
            results.Should().BeOfType<List<NewsStory>>();
            results.Should().HaveCount(expectedPageLimit);

            // Verify Start
            var resultsList = (List<NewsStory>)results;
            resultsList[0].Id.Should().Be(_testStoryKeysList[expectedStartAtIndex]);

            // Verify End
            var endIndx = (expectedStartAtIndex + expectedPageLimit) - 1;
            resultsList[(expectedPageLimit - 1)].Id.Should().Be(_testStoryKeysList[endIndx]);
        }


        [Fact]
        public async Task Test_GetNewsStoryListAsync_Cache_DatasourceCalledOnce_Async()
        {
            // Arrange
            SetupDatasourceMock();
            var expectedPageLimit = 5;
            var expectedStartAtIndex = 0;

            // Act
            var results = await _newsSourceHackerNews.GetNewsStoryListAsync(expectedStartAtIndex, expectedPageLimit);
            _ = await _newsSourceHackerNews.GetNewsStoryListAsync(expectedStartAtIndex, expectedPageLimit);

            // Assert
            _datasourceMock.Verify(x => x.GetStoryDetailAsync<HackerNewsItem>(_testStoryKeysList[expectedStartAtIndex]), Times.Once);
        }

    }
}