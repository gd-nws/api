using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNews.Models;
using GoodNews.Models.DBModels;
using GoodNews.Models.DBModels.Postgres;
using Microsoft.EntityFrameworkCore;
using Models.DBModels;

namespace GoodNews.Repositories.Postgres
{
  public class NewsHeadlineRepository : PostgresRepository, INewsHeadlineRepository
  {
    public NewsHeadlineRepository(GoodNewsDBContext db) : base(db) { }

    class AnnotatedHeadlineResult
    {
      public NewsHeadline Headline { get; set; }
      public AnnotationGrouping Annotation { get; set; }
    }

    class AnnotationGrouping
    {
      public int PositiveVotes { get; set; }
      public int NegativeVotes { get; set; }
      public long HeadlineId { get; set; }
    }

    public async Task<IList<INewsHeadline>> FetchHeadlinesBySentiment(HeadlineSentiment sentiment, int dateOffset,
        int limit = 10, int offset = 10)
    {
      var isPositive = sentiment == HeadlineSentiment.POSITIVE;
      var predictedClass = isPositive ? 1 : 0;

      var d = DateTime.Now;
      d.AddDays(dateOffset);

      var query = (
          from h in Db.NewsHeadlines
          join a in (
            from a in Db.Annotations
            group a by a.NewsHeadline.Id into headline
            select new AnnotationGrouping()
            {
              HeadlineId = headline.Key,
              NegativeVotes = headline.Sum(h => h.Negative),
              PositiveVotes = headline.Sum(h => h.Positive)
            }
            ) on h.Id equals a.HeadlineId into aj
          from ans in aj.DefaultIfEmpty()
          where
            h.PredictedClass == predictedClass && h.PublishedAt.Date == d.Date
          select new AnnotatedHeadlineResult() { Annotation = ans, Headline = h }
      );

      var results = isPositive ?
          await QueryAndSortByPositivity(query, offset, limit) :
          await QueryAndSortByNegativity(query, offset, limit);

      return ParseToAnnotatedHeadlines(results);
    }

    private async Task<List<AnnotatedHeadlineResult>> QueryAndSortByPositivity(IQueryable<AnnotatedHeadlineResult> query, int offset, int limit)
    {
      var result = await (
              query
          )
          .OrderByDescending(r => r.Headline.SemanticValue)
          .Skip(offset)
          .Take(limit)
          .ToListAsync();

      return result;
    }

    private async Task<List<AnnotatedHeadlineResult>> QueryAndSortByNegativity(IQueryable<AnnotatedHeadlineResult> query, int offset, int limit)
    {
      var result = await (
              query
          )
          .OrderBy(r => r.Headline.SemanticValue)
          .Skip(offset)
          .Take(limit)
          .ToListAsync();

      return result;
    }

    public async Task<INewsHeadline> GetHeadline(int headlineId)
    {
      var query = (
          from h in Db.NewsHeadlines
          join a in (
            from a in Db.Annotations
            group a by a.NewsHeadline.Id into headline
            select new AnnotationGrouping()
            {
              HeadlineId = headline.Key,
              NegativeVotes = headline.Sum(h => h.Negative),
              PositiveVotes = headline.Sum(h => h.Positive)
            }
            ) on h.Id equals a.HeadlineId into aj
          from ans in aj.DefaultIfEmpty()
          where h.Id == headlineId
          select new AnnotatedHeadlineResult() { Annotation = ans, Headline = h }
      );

      var results = await query.ToListAsync();
      return ParseToAnnotatedHeadlines(results).FirstOrDefault();
    }

    public async Task<int> FetchHeadlinesBySentimentCount(HeadlineSentiment sentiment, int dateOffset)
    {
      var predictedClass = sentiment == HeadlineSentiment.POSITIVE ? 1 : 0;

      var d = DateTime.Now;
      d.AddDays(dateOffset);

      var query = (
          from h in Db.NewsHeadlines
          where h.PredictedClass == predictedClass && h.PublishedAt.Date == d.Date
          select h.Id
      );

      var count = await query.CountAsync();
      return count;
    }

    public async Task<IList<INewsHeadline>> SearchHeadlines(HeadlineSentiment sentiment, string term, int limit = 10,
        int offset = 0)
    {
      var isPositive = sentiment == HeadlineSentiment.POSITIVE;
      var predictedClass = isPositive ? 1 : 0;
      var query = (
          from h in Db.NewsHeadlines
          join a in (
            from a in Db.Annotations
            group a by a.NewsHeadline.Id into headline
            select new AnnotationGrouping()
            {
              HeadlineId = headline.Key,
              NegativeVotes = headline.Sum(h => h.Negative),
              PositiveVotes = headline.Sum(h => h.Positive)
            }
        ) on h.Id equals a.HeadlineId into aj
          from ans in aj.DefaultIfEmpty()
          where h.Headline.ToLower().Contains(term.ToLower()) && h.PredictedClass == predictedClass
          select new AnnotatedHeadlineResult() { Annotation = ans, Headline = h }
      );

      var results = isPositive ? await QueryAndSortByPositivity(query, offset, limit) : await QueryAndSortByNegativity(query, offset, limit);
      return ParseToAnnotatedHeadlines(results);
    }

    public async Task<int> SearchHeadlinesCount(HeadlineSentiment sentiment, string term)
    {
      var predictedClass = sentiment == HeadlineSentiment.POSITIVE ? 1 : 0;
      var query = (
          from h in Db.NewsHeadlines where h.Headline.ToLower().Contains(term.ToLower()) && h.PredictedClass == predictedClass select h.Id
      );

      return await query.CountAsync();
    }

    private AnnotatedHeadline[] ParseToAnnotatedHeadlines(List<AnnotatedHeadlineResult> results)
    {
      var formatted = (
        from r in results
        select new AnnotatedHeadline()
        {
          Id = r.Headline.Id,
          Headline = r.Headline.Headline,
          Hashcode = r.Headline.Hashcode,
          PublishedAt = r.Headline.PublishedAt,
          PredictedClass = r.Headline.PredictedClass,
          Origin = r.Headline.Origin,
          Link = r.Headline.Link,
          SemanticValue = r.Headline.SemanticValue,
          DisplayImage = r.Headline.DisplayImage,
          CreatedAt = r.Headline.CreatedAt,
          Pos = r.Headline.Pos,
          Neg = r.Headline.Neg,
          Nue = r.Headline.Nue,
          PositiveVotes = r.Annotation.PositiveVotes,
          NegativeVotes = r.Annotation.NegativeVotes
        }).ToArray();

      return formatted;
    }

    public Task UpdateHeadline(INewsHeadline headline)
    {
      throw new NotImplementedException();
    }
  }
}