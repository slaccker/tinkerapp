using System.Threading.Tasks;
using System.Collections.Generic;
using Web.NewsSources.HackerNews;

namespace Web.Interfaces
{
    public interface INewsSourceDataSource<TClient>
    {
        /// <summary>
        /// Create Client for baseURL
        /// </summary>
        /// <param name="baseURL"></param>
        /// <returns></returns>
        TClient CreateClient(string baseURL);

        /// <summary>
        /// Get existing Client
        /// </summary>
        /// <remarks>Throws InvalidOperationException if Client does not exist</remarks>
        /// <returns></returns>
        TClient GetClient();

        /// <summary>
        /// Get ID of Newest Story
        /// </summary>
        /// <returns></returns>
        Task<int> GetNewestStoryID();

        /// <summary>
        /// Get Keys for Newest Stories
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<int>> GetStoryKeysAsyncTypeNewest();

        /// <summary>
        /// Get Story Detail by Key
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="TStoryDetail"></typeparam>
        /// <returns></returns>        
        Task<TStoryDetail> GetStoryDetailAsync<TStoryDetail>(int key);

    }
}