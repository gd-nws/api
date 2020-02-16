using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNews.Models;

namespace GoodNews.Repositories.Postgres
{
    public class NewsHeadlineRepository : PostgresRepository, INewsHeadlineRepository
    {
        public NewsHeadlineRepository(GoodNewsDBContext db) : base(db) { }

        public Task<IList<NewsHeadline>> FetchHeadlinesBySentiment(HeadlineSentiment sentiment, int dateOffset, int limit = 10, int offset = 10)
        {
            throw new System.NotImplementedException();
        }

        public Task<NewsHeadline> GetHeadline(int headlineId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> FetchHeadlinesBySentimentCount(HeadlineSentiment sentiment, int dateOffset)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<NewsHeadline>> SearchHeadlines(HeadlineSentiment sentiment, string term, int limit = 10, int offset = 0)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> SearchHeadlinesCount(string term)
        {
            throw new System.NotImplementedException();
        }
    }
}