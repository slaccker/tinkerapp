namespace Web.NewsSources.HackerNews
{
    public static class NewsSourceConfig
    {

        /// <summary>
        /// News Source Identifier
        /// </summary>
        /// <value></value>
        public static string NewsSourceID = "hackernews";

        /// <summary>
        /// News source name
        /// </summary>
        /// <value></value>
        public static string NewsSourceName = "Hacker News";

        /// <summary>
        /// Api Base URL
        /// </summary>
        /// <value></value>
        public static string ApiBaseURL = "https://hacker-news.firebaseio.com/v0";

        //public string TopStoryPartialURL { get; set; } = "topstories";

        /// <summary>
        /// Partial URL for Newest Stories
        /// </summary>
        /// <value></value>
        public static string NewestStoryPartialURL = "newstories";

        /// <summary>
        /// Partial URL to get Story detail
        /// </summary>
        /// <remarks>Uses substitution {NewsSourceStoryID}.
        /// <value></value>
        public static string StoryDetailPartialURL = "item/{NewsSourceStoryID}";

        /// <summary>
        /// Partial URL to get User detail
        /// </summary>
        public static string UserDetailPartialURL = "user";

        /// <summary>
        /// Partial URL to get Max Item ID
        /// </summary>
        public static string MaxItemIDPartialURL = "maxitem";

        /// <summary>
        /// Default Story Limit
        /// </summary>
        public static int DefaultStoryLimit = 500;
    }
}