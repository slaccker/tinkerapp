
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.NewsSources.HackerNews
{
    public class HackerNewsItem
    {

        /// <summary>
        /// The item's unique id.
        /// </summary>
        public int id;

        /// <summary>
        /// true if the item is deleted.
        /// </summary>
        public bool deleted;

        /// <summary>
        /// Type of item. One of "job", "story", "comment", "poll", or "pollopt"
        /// </summary>
        public string type;

        /// <summary>
        /// The username of the item's author.
        /// </summary>
        public string by;

        /// <summary>
        /// Creation date of the item, in Unix Time.
        /// </summary>
        public long time;

        /// <summary>
        /// The comment, story or poll text. HTML.
        /// </summary>
        public string text;

        /// <summary>
        /// true if the item is dead.
        /// </summary>
        public bool dead;

        /// <summary>
        /// The comment's parent: either another comment or the relevant story.
        /// </summary>
        public string parent;

        /// <summary>
        /// The pollopt's associated poll.
        /// </summary>
        public int poll;

        /// <summary>
        /// The ids of the item's comments, in ranked display order.
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <returns></returns>
        public List<int> kids = new List<int>();

        /// <summary>
        /// The URL of the story.
        /// </summary>
        public string url;

        /// <summary>
        /// The story's score, or the votes for a pollopt.
        /// </summary>
        public int score;

        /// <summary>
        /// title of story, poll, or job. HTML.
        /// </summary>
        public string title;

        /// <summary>
        /// A list of related pollopts, in display order
        /// </summary>
        public List<int> parts = new List<int>();

        /// <summary>
        /// Total comment count for stories, or polls
        /// </summary>
        public int descendants;
    }
}