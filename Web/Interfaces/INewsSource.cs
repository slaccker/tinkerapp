using System;
using System.Collections.Generic;
using System.Linq;
using Web.Models;
using System.Threading.Tasks;

namespace Web.Interfaces
{
    public interface INewsSource
    {

        /// <summary>
        /// Identifier string for News Source
        /// </summary>
        /// <value></value>
        public string Id { get; }

        /// <summary>
        /// Human readable name for News Source
        /// </summary>
        /// <value></value>
        public string Name { get; }

        /// <summary>
        /// List of News Stories
        /// </summary>
        /// <param name="startAtOffset">Index of first item</param>
        /// <param name="perPageLimit">Number of items on page</param>
        /// <returns></returns>
        public Task<IEnumerable<NewsStory>> GetNewsStoryListAsync(int startAtOffset = -1, int perPageLimit = -1);

    }
}