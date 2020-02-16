using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNews.Models;

namespace GoodNews.Repositories
{
    public interface INewsHeadlineRepository
    {
        Task<IList<NewsHeadline>> FetchHeadlinesBySentiment(HeadlineSentiment sentiment, int dateOffset, int limit = 10,
            int offset = 10);

        Task<NewsHeadline> GetHeadline(int headlineId);

        Task<int> FetchHeadlinesBySentimentCount(HeadlineSentiment sentiment, int dateOffset);

        Task<IList<NewsHeadline>> SearchHeadlines(HeadlineSentiment sentiment, string term, int limit = 10,
            int offset = 0);

        Task<int> SearchHeadlinesCount(string term);
    }
}