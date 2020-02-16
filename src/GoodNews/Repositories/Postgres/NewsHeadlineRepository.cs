using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNews.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodNews.Repositories.Postgres
{
    public class NewsHeadlineRepository : PostgresRepository, INewsHeadlineRepository
    {
        public NewsHeadlineRepository(GoodNewsDBContext db) : base(db)
        {
        }

        public async Task<IList<NewsHeadline>> FetchHeadlinesBySentiment(HeadlineSentiment sentiment, int dateOffset,
            int limit = 10, int offset = 10)
        {
            var op = sentiment == HeadlineSentiment.POSITIVE ? ">" : "=";
            var sort = sentiment == HeadlineSentiment.POSITIVE ? "DESC" : "ASC";
            
            var headlines = await Db.NewsHeadlines.FromSqlRaw($@"
                SELECT h.id,
                       h.headline,
                       h.predicted_class,
                       h.origin,
                       h.link,
                       h.semantic_value,
                       h.published_at,
                       h.display_image,
                       h.created_at,
                       h.pos,
                       h.neg,
                       h.neu,
                       h.hashcode,
                       COALESCE(a.positive_votes, 0) as positive_votes,
                       COALESCE(a.negative_votes, 0) as negative_votes
                FROM good_news.headlines h
                LEFT JOIN (
                    SELECT a.headline_id,
                        SUM(a.positive) as positive_votes,
                        SUM(a.negative) as negative_votes
                    FROM good_news.annotations a
                    GROUP BY a.headline_id
                ) a on h.id = a.headline_id
                WHERE h.predicted_class {op} 0
                  AND h.published_at = DATE(NOW() - INTERVAL '{dateOffset} DAY')
                ORDER BY h.semantic_value {sort}
                LIMIT {limit} OFFSET {offset}
            ").AsNoTracking().ToListAsync();

            return headlines;
        }

        public async Task<NewsHeadline> GetHeadline(int headlineId)
        {
            var result = await Db.NewsHeadlines.FromSqlRaw($@"
                SELECT h.id,
                       h.headline,
                       h.predicted_class,
                       h.origin,
                       h.link,
                       h.semantic_value,
                       h.published_at,
                       h.display_image,
                       h.created_at,
                       h.pos,
                       h.neg,
                       h.neu,
                       h.hashcode,
                       COALESCE(a.positive_votes, 0) as positive_votes,
                       COALESCE(a.negative_votes, 0) as negative_votes
                FROM good_news.headlines h
                LEFT JOIN (
                    SELECT a.headline_id,
                        SUM(a.positive) as positive_votes,
                        SUM(a.negative) as negative_votes
                    FROM good_news.annotations a
                    GROUP BY a.headline_id
                ) a on h.id = a.headline_id
                WHERE h.id = {headlineId}
            ").AsNoTracking().ToListAsync();

            return result.Count > 0 ? result.First() : null;
        }

        public async Task<int> FetchHeadlinesBySentimentCount(HeadlineSentiment sentiment, int dateOffset)
        {
            var op = sentiment == HeadlineSentiment.POSITIVE ? ">" : "=";
            return await Db.NewsHeadlines.FromSqlRaw($@"
                SELECT *
			    FROM good_news.headlines h
			    WHERE 
			      h.predicted_class {op} 0
                AND h.published_at = DATE(NOW() - INTERVAL '{dateOffset} DAY')
            ").AsNoTracking().CountAsync();
        }

        public async Task<IList<NewsHeadline>> SearchHeadlines(HeadlineSentiment sentiment, string term, int limit = 10,
            int offset = 0)
        {
            var sort = sentiment == HeadlineSentiment.POSITIVE ? "DESC" : "ASC";
            return await Db.NewsHeadlines.FromSqlRaw($@"
                SELECT h.id,
                       h.headline,
                       h.predicted_class,
                       h.origin,
                       h.link,
                       h.semantic_value,
                       h.published_at,
                       h.display_image,
                       h.created_at,
                       h.pos,
                       h.neg,
                       h.neu,
                       h.hashcode,
                       COALESCE(a.positive_votes, 0) as positive_votes,
                       COALESCE(a.negative_votes, 0) as negative_votes
                FROM good_news.headlines h
                LEFT JOIN (
                    SELECT a.headline_id,
                        SUM(a.positive) as positive_votes,
                        SUM(a.negative) as negative_votes
                    FROM good_news.annotations a
                    GROUP BY a.headline_id
                ) a on h.id = a.headline_id
                WHERE
                    h.headline LIKE '%{term}%'
                ORDER BY h.semantic_value {sort}
                LIMIT {limit} OFFSET {offset}
                
            ").ToListAsync();
        }

        public async Task<int> SearchHeadlinesCount(string term)
        {
            return await Db.NewsHeadlines.FromSqlRaw($@"
                SELECT 1
                FROM good_news.headlines h
                WHERE
                    h.headline LIKE '%{term}%'
            ").CountAsync();
        }
    }
}