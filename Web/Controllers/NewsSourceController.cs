using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Web.Models;
using Web.Interfaces;
using Web.Settings;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsSourceController : ControllerBase
    {
        #region Fields 

        private readonly ILogger<NewsSourceController> _logger;
        private INewsSource _defaultNewsSource;
        private readonly DefaultsOptions _options;

        #endregion

        public NewsSourceController(ILogger<NewsSourceController> logger, INewsSource defaultNewsSource, IOptions<DefaultsOptions> options)
        {
            _options = options.Value;
            _logger = logger;
            _defaultNewsSource = defaultNewsSource;
        }

        [HttpGet]
        public async Task<IEnumerable<NewsStory>> Get(int startAtOffset = -1, int perPageLimit = -1)
        {
            if (perPageLimit < 1)
            {
                // !NOTE!: Angular handles the pagination
                perPageLimit = _options.PerPageStories;
            }
            var stories = await _defaultNewsSource.GetNewsStoryListAsync(startAtOffset, perPageLimit);

            return stories.ToArray();
        }
    }
}