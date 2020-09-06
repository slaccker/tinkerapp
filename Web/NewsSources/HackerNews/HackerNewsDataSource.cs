using Web.Interfaces;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Web.NewsSources.HackerNews
{
    public class HackerNewsDataSource : INewsSourceDataSource<FirebaseClient>
    {
        #region Constants 

        private const int RETRY_TIMES = 3;

        #endregion
        #region Fields

        /// <summary>
        /// FirebaseClient. Datasource that reads from Firebase.io
        /// </summary>
        private FirebaseClient _firebaseClient;

        private readonly ILogger<HackerNewsDataSource> _logger;

        /// <summary>
        /// Base URL
        /// </summary>
        private string _baseURL;

        #endregion

        public HackerNewsDataSource(ILogger<HackerNewsDataSource> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public FirebaseClient CreateClient(string baseURL)
        {
            if (_baseURL == null)
            {
                _baseURL = baseURL;

                _firebaseClient = new FirebaseClient(_baseURL);
                _logger.LogInformation($"FirebaseClient created for URL: {_baseURL}");
            }

            return _firebaseClient;
        }

        /// <inheritdoc/>
        public FirebaseClient GetClient()
        {
            if (_firebaseClient == null)
                throw new System.InvalidOperationException();

            return _firebaseClient;
        }

        /// <inheritdoc/>
        public async Task<int> GetNewestStoryID()
        {
            return await GetMaxItemID();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<int>> GetStoryKeysAsyncTypeNewest() => await GetStoryKeysAsync(NewsSourceConfig.NewestStoryPartialURL);

        /// <summary>
        /// Get Enumerable (list) of Stories from Resource
        /// </summary>
        /// <param name="storyResource">Only "newest"</param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetStoryKeysAsync(string storyResource)
        {
            var storyKeys = await GetClient()
                .Child(storyResource)
                .OrderByKey()
                .OnceSingleAsync<List<int>>();
            _logger.LogDebug("Get Newest StoryKeys: ", storyKeys);

            return storyKeys;
        }

        /// <inheritdoc/>
        public async Task<TStoryDetail> GetStoryDetailAsync<TStoryDetail>(int key)
        {
            TStoryDetail detail = default(TStoryDetail);

            var itemResource = NewsSourceConfig.StoryDetailPartialURL;
            itemResource = itemResource.Replace("{NewsSourceStoryID}", key.ToString());

            var retry = 0;
            while (detail == null && retry <= RETRY_TIMES)
            {
                try
                {
                    detail = await GetClient()
                    .Child(itemResource)
                    .OnceSingleAsync<TStoryDetail>();
                    _logger.LogDebug("GetStoryDetail {itemResource}: {detail} ", itemResource, detail);
                }
                catch (System.Net.Http.HttpRequestException e)
                {
                    _logger.LogError("[HttpRequestException] GetStoryDetail error: ", e.Message);
                    retry++;
                }
                catch (Firebase.Database.FirebaseException e)
                {

                    _logger.LogError("[FirebaseException] GetStoryDetail error: ", e.Message);
                    retry++;
                }
            }

            return detail;
        }

        #region Helpers

        /// <summary>
        /// Get Max Item ID for Story
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetMaxItemID()
        {
            var result = await GetClient()
                .Child(NewsSourceConfig.MaxItemIDPartialURL)
                .OnceSingleAsync<int>();

            return result;
        }

        #endregion

    }
}