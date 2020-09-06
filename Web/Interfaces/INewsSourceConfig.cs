namespace Web.Interfaces
{
    public interface INewsSourceConfig
    {

        /// <summary>
        /// News Source Identifier
        /// </summary>
        /// <value></value>
        public string NewsSourceID { get; }

        /// <summary>
        /// News source name
        /// </summary>
        /// <value></value>
        public string NewsSourceName { get; }

        /// <summary>
        /// Api Base URL
        /// </summary>
        /// <value></value>
        public string ApiBaseURL { get; }

    }
}