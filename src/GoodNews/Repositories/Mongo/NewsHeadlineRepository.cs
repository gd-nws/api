using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNews.Models;
using GoodNews.Models.DBModels;
using GoodNews.Models.Settings;
using Models.DBModels.Mongo;
using MongoDB.Driver;

namespace GoodNews.Repositories.Mongo
{
  class NewsHeadlineRepository : INewsHeadlineRepository
  {
    private readonly IMongoCollection<MongoNewsHeadline> _headlines;

    public NewsHeadlineRepository(IMongoSettings settings)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _headlines = database.GetCollection<MongoNewsHeadline>(settings.HeadlinesCollectionName);
    }
    public async Task<IList<INewsHeadline>> FetchHeadlinesBySentiment(HeadlineSentiment sentiment, int dateOffset, int limit = 10, int offset = 0)
    {
      var isPositive = sentiment == HeadlineSentiment.POSITIVE;
      var predictedClass = isPositive ? 1 : 0;
      var targetDate = DateTime.Now.AddDays(-1 * dateOffset).Date;
      var endDate = targetDate.AddDays(1).Date;

      var filter = _headlines
        .Find(
          h => h.PublishedAt >= targetDate &&
            h.PublishedAt < endDate &&
            h.PredictedClass == predictedClass
        ).Limit(limit).Skip(offset);

      var headlines = isPositive ?
        await filter.SortByDescending(h => h.SemanticValue).ToListAsync() :
        await filter.SortBy(h => h.SemanticValue).ToListAsync();

      return headlines.Cast<INewsHeadline>().ToList();
    }

    public async Task<int> FetchHeadlinesBySentimentCount(HeadlineSentiment sentiment, int dateOffset)
    {
      var predictedClass = sentiment == HeadlineSentiment.POSITIVE ? 1 : 0;
      var targetDate = DateTime.Now.AddDays(-1 * dateOffset).Date;
      var endDate = targetDate.AddDays(1).Date;

      var count = await _headlines
        .Find(
          h => h.PublishedAt >= targetDate &&
          h.PublishedAt < endDate &&
          h.PredictedClass == predictedClass
        ).CountDocumentsAsync();

      return (int)count;
    }

    public async Task<INewsHeadline> GetHeadline(int headlineId)
    {
      var headline = await _headlines.Find(h => h.Id == headlineId).SingleOrDefaultAsync();
      return headline;
    }

    public async Task<IList<INewsHeadline>> SearchHeadlines(HeadlineSentiment sentiment, string term, int limit = 10, int offset = 0)
    {
      var isPositive = sentiment == HeadlineSentiment.POSITIVE;
      var predictedClass = isPositive ? 1 : 0;

      var filter = _headlines
        .Find(
          h => h.PredictedClass == predictedClass &&
            h.Headline.ToLower().Contains(term.ToLower())
        ).Limit(limit).Skip(offset);

      var headlines = isPositive ?
        await filter.SortByDescending(h => h.SemanticValue).ToListAsync() :
        await filter.SortBy(h => h.SemanticValue).ToListAsync();

      return headlines.Cast<INewsHeadline>().ToList();
    }

    public async Task<int> SearchHeadlinesCount(HeadlineSentiment sentiment, string term)
    {
      var isPositive = sentiment == HeadlineSentiment.POSITIVE;
      var predictedClass = isPositive ? 1 : 0;

      var count = await _headlines
        .Find(
          h => h.PredictedClass == predictedClass &&
            h.Headline.ToLower().Contains(term.ToLower())
        ).CountDocumentsAsync();

      return (int)count;
    }
  }
}