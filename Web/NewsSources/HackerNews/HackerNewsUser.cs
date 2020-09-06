using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.NewsSources.HackerNews
{
    public class HackerNewsUser
    {

        // The user's unique username. Case-sensitive. Required.
        public string id;

        // Delay in minutes between a comment's creation and its visibility to other users.
        public long delay;

        // Creation date of the user, in Unix Time.
        public long created;

        // The user's karma.
        public int karma;

        // The user's optional self-description. HTML.
        public string about;

        // List of the user's stories, polls and comments.
        public List<int> submitted = new List<int>();
    }
}
