using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNews.Models;
using GoodNews.Models.DBModels;

namespace GoodNews.Repositories
{
  public interface INewsHeadlineRepository
  {
    Task<IList<INewsHeadline>> FetchHeadlinesBySentiment(HeadlineSentiment sentiment, int dateOffset, int limit = 10,
        int offset = 10);

    Task<INewsHeadline> GetHeadline(string headlineId);

    Task<int> FetchHeadlinesBySentimentCount(HeadlineSentiment sentiment, int dateOffset);

    Task<IList<INewsHeadline>> SearchHeadlines(HeadlineSentiment sentiment, string term, int limit = 10,
        int offset = 0);

    Task<int> SearchHeadlinesCount(HeadlineSentiment sentiment, string term);

    Task UpdateHeadline(INewsHeadline headline);
  }
}