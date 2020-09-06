using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;

using Firebase.Database;

using Web.NewsSources;
using Web.Interfaces;
using Microsoft.Extensions.Logging;
using Web.NewsSources.HackerNews;

namespace UnitTests.NewsSourcesTests.NewsSourceHackerNewsTests
{
    public class GetStoryKeysAsyncTests : TestBase
    {
        #region Fields

        private List<int> _testStoryKeysList;

        #endregion

        public GetStoryKeysAsyncTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _newsSourceHackerNews.SetDataSourceBaseURL(_mockURL);

            _testStoryKeysList = new List<int>() { 111, 222, 333, 444 };
            _datasourceMock.Setup(x => x.GetStoryKeysAsyncTypeNewest()).ReturnsAsync(_testStoryKeysList);
        }

        [Fact]
        public async Task Test_GetStoryKeysAsync_ReturnList_Async()
        {
            // Arrange


            // Act
            var results = await _newsSourceHackerNews.GetStoryKeysAsync();

            // Assert
            results.Should().BeOfType<List<int>>();
            results.Should().HaveCount(_testStoryKeysList.Count);
        }

        [Fact]
        public async Task Test_GetStoryKeysAsync_ReturnEmptyList_Async()
        {
            // Arrange
            var emptyList = new List<int>();
            _datasourceMock.Setup(x => x.GetStoryKeysAsyncTypeNewest()).ReturnsAsync(emptyList);

            // Act
            var results = await _newsSourceHackerNews.GetStoryKeysAsync();

            // Assert
            results.Should().BeOfType<List<int>>();
            results.Should().HaveCount(emptyList.Count);
        }
    }
}