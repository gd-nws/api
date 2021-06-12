using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNews.Models;
using GoodNews.Models.DBModels;
using GoodNews.Models.DBModels.Mongo;
using GoodNews.Models.Settings;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GoodNews.Repositories.Mongo
{
    class NewsHeadlineRepository : BaseRepository<INewsHeadline>, INewsHeadlineRepository
    {
        private readonly IMongoCollection<MongoNewsHeadline> _headlines;

        public NewsHeadlineRepository(IMongoSettings settings) : base(settings)
        {

            _headlines = base._db.GetCollection<MongoNewsHeadline>(settings.HeadlinesCollectionName);

            if (!BsonClassMap.IsClassMapRegistered(typeof(MongoNewsHeadline)))
                BsonClassMap.RegisterClassMap<MongoNewsHeadline>();

            if (!BsonClassMap.IsClassMapRegistered(typeof(HeadlineVotes)))
                BsonClassMap.RegisterClassMap<HeadlineVotes>();
        }

        public override Task<INewsHeadline> Create(INewsHeadline t)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<INewsHeadline>> FetchHeadlinesBySentiment(
          HeadlineSentiment sentiment, int dateOffset, int limit = 10, int offset = 0)
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

        public async override Task<INewsHeadline> GetById(string id)
        {
            var headline = await _headlines.Find(h => h.Id.Equals(id)).SingleOrDefaultAsync();
            return headline;
        }

        public async Task<IList<INewsHeadline>> SearchHeadlines(
          HeadlineSentiment sentiment, string term, int limit = 10, int offset = 0)
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

        /// <summary>
        /// Update a headline.
        /// Replaces the current headline with the one passed in.
        /// </summary>
        /// <param name="headline"></param>
        /// <returns></returns>
        public async Task UpdateHeadline(INewsHeadline headline)
        {
            await _headlines.ReplaceOneAsync(h => h.Id == headline.Id, (MongoNewsHeadline)headline);
        }
    }
}