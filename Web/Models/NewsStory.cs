namespace Web.Models
{
    public class NewsStory
    {
        public int Id { get; set; }

        /// <summary>
        /// Story Title
        /// </summary>
        /// <value></value>
        public string Title { get; set; }

        /// <summary>
        /// Story URL
        /// </summary>
        /// <value></value>
        public string URL { get; set; }

        /// <summary>
        /// Story Detail
        /// </summary>
        /// <value></value>
        public string Detail { get; set; }
    }
}