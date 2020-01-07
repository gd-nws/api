using System.Collections.Generic;

namespace GoodNews.Models.Responses
{
    public class HeadlinesResponse
    {
        public IList<NewsHeadline> Headlines { get; set; }
        public int Count { get; set; }
    }
}