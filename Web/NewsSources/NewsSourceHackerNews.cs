using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Web.Interfaces;
using Web.Models;
using Web.NewsSources.HackerNews;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Web.NewsSources
{
    public class NewsSourceHackerNews : INewsSource
    {
        #region Constants

        /// <summary>
        /// Maximum TTL for cached item
        /// </summary>
        private const int CACHE_TTL_ITEM_MAX_MINS = 30;

        #endregion
        #region Fields

        /// <summary>
        /// Datasource provider
        /// </summary>
        private INewsSourceDataSource<FirebaseClient> _datasource;

        /// <summary>
        /// In-memory cache
        /// </summary>
        private IMemoryCache _cache;

        /// <summary>
        /// Maximum Item ID
        /// </summary>
        /// <remarks>Stores identifier for most recent Hacker News item</remarks>
        /// <value></value>
        private int _maxItemID = -1;

        private readonly ILogger<NewsSourceHackerNews> _logger;

        #endregion

        public string Id => NewsSourceConfig.NewsSourceID;

        public string Name => NewsSourceConfig.NewsSourceName;


        public NewsSourceHackerNews(ILogger<NewsSourceHackerNews> logger, INewsSourceDataSource<FirebaseClient> dataSource, IMemoryCache cache)
        {
            _logger = logger;

            _cache = cache;

            _datasource = dataSource;
            SetDataSourceBaseURL(NewsSourceConfig.ApiBaseURL);
        }

        public void SetDataSourceBaseURL(string baseURL)
        {
            _datasource.CreateClient(baseURL);
        }

        public async Task<int> GetMaxItemIDAsync()
        {
            _maxItemID = await _datasource.GetNewestStoryID();

            return _maxItemID;
        }

        public async Task<IEnumerable<NewsStory>> GetNewsStoryListAsync(int startAtOffset = -1, int perPageLimit = -1)
        {
            var newestStoryList = new List<NewsStory>();

            // Get full list of keys
            var storyKeys = await GetStoryKeysAsync();

            int i = (startAtOffset <= 0 ? 0 : startAtOffset);
            var pageStopIndex = perPageLimit <= 0 ? NewsSourceConfig.DefaultStoryLimit : (startAtOffset + perPageLimit);
            _logger.LogDebug("Paging startAtOffset: {startAtOffset}, page: {page} [pageStopIndex: {pageStopIndex}] ", startAtOffset, perPageLimit, pageStopIndex);

            for (; i < storyKeys.Count; i++)
            {
                if (i >= pageStopIndex) break;

                // Jump to key at startAtOffset
                var storyKey = storyKeys[i];

                // Check for key in cache
                NewsStory cacheEntry;
                if (!_cache.TryGetValue(storyKey, out cacheEntry))
                {
                    // Get detail for key
                    var storyDetail = await GetStoryDetailAsync(storyKey);
                    if (storyDetail == null)
                    {
                        _logger.LogInformation("Story {storyKey} has empty detail. ", storyKey);
                        continue;
                    }

                    // Create entry
                    var newsStory = new NewsStory()
                    {
                        Id = storyKey,
                        Title = storyDetail.title,
                        URL = storyDetail.url ?? "",
                        Detail = storyDetail.text ?? ""
                    };

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSize(1)
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_TTL_ITEM_MAX_MINS));

                    // Store in cache
                    cacheEntry = _cache.Set(storyKey, newsStory, cacheEntryOptions);
                }
                newestStoryList.Add(cacheEntry);
            }

            return newestStoryList;
        }

        public async Task<List<int>> GetStoryKeysAsync()
        {
            // Get detail for key
            var storyKeys = await _datasource.GetStoryKeysAsyncTypeNewest();
            _logger.LogDebug("StoryKeys {storyCateory}: {count} ", "newest", storyKeys.Count().ToString());

            return (List<int>)storyKeys;
        }

        public async Task<HackerNewsItem> GetStoryDetailAsync(int storyKey)
        {
            // Get detail for key
            var storyDetail = await _datasource.GetStoryDetailAsync<HackerNewsItem>(storyKey);
            _logger.LogDebug("StoryDetail {storyKey}: {storyDetail} ", storyKey, storyDetail);

            return storyDetail;
        }

    }
}