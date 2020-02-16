using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNews.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodNews.Repositories.MySQL
{
    public class NewsHeadlineRepository : MySqlRepository, INewsHeadlineRepository
    {
        public NewsHeadlineRepository(GoodNewsDBContext db) : base(db)
        {
        }

        /// <summary>
        /// Fetch headlines sorted by sentiment.
        /// </summary>
        /// <param name="sentiment"></param>
        /// <param name="dateOffset"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<IList<NewsHeadline>> FetchHeadlinesBySentiment(HeadlineSentiment sentiment, int dateOffset,
            int limit = 10, int offset = 0)
        {
            var op = sentiment == HeadlineSentiment.POSITIVE ? ">" : "=";
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
                FROM headlines h
                LEFT JOIN (
                    SELECT a.headline_id,
                        SUM(a.positive) as positive_votes,
                        SUM(a.negative) as negative_votes
                    FROM annotations a
                    GROUP BY a.headline_id
                ) a on h.id = a.headline_id
                WHERE h.predicted_class {op} 0
                  AND DATE(h.published_at) = CURDATE() - INTERVAL {dateOffset} DAY
                ORDER BY h.semantic_value {sort}
                LIMIT {limit} OFFSET {offset}
            ").AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Fetch a single headline.
        /// </summary>
        /// <param name="headlineId"></param>
        /// <returns></returns>
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
                FROM headlines h
                LEFT JOIN (
                    SELECT a.headline_id,
                        SUM(a.positive) as positive_votes,
                        SUM(a.negative) as negative_votes
                    FROM annotations a
                    GROUP BY a.headline_id
                ) a on h.id = a.headline_id
                WHERE h.id = {headlineId}
            ").AsNoTracking().ToListAsync();

            return result.Count > 0 ? result.First() : null;
        }

        /// <summary>
        /// Fetch headlines sorted by sentiment count.
        /// </summary>
        /// <param name="sentiment"></param>
        /// <param name="dateOffset"></param>
        /// <returns></returns>
        public async Task<int> FetchHeadlinesBySentimentCount(HeadlineSentiment sentiment, int dateOffset)
        {
            var op = sentiment == HeadlineSentiment.POSITIVE ? ">" : "=";
            return await Db.NewsHeadlines.FromSqlRaw($@"
                SELECT *
			    FROM headlines h
			    WHERE 
			      h.predicted_class {op} 0
                AND DATE(h.published_at) = CURDATE() - INTERVAL {dateOffset} DAY
            ").AsNoTracking().CountAsync();
        }

        /// <summary>
        /// Search headlines for a string.
        /// </summary>
        /// <param name="sentiment"></param>
        /// <param name="term"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public async Task<IList<NewsHeadline>> SearchHeadlines(HeadlineSentiment sentiment, string term, int limit = 10,
            int offset = 10)
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
                FROM headlines h
                LEFT JOIN (
                    SELECT a.headline_id,
                        SUM(a.positive) as positive_votes,
                        SUM(a.negative) as negative_votes
                    FROM annotations a
                    GROUP BY a.headline_id
                ) a on h.id = a.headline_id
                WHERE
                    h.headline LIKE '%{term}%'
                ORDER BY h.semantic_value {sort}
                LIMIT {limit} OFFSET {offset}
                
            ").ToListAsync();
        }

        /// <summary>
        /// Get a count for a search query
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public async Task<int> SearchHeadlinesCount(string term)
        {
            return await Db.NewsHeadlines.FromSqlRaw($@"
                SELECT 1
                FROM headlines h
                WHERE
                    h.headline LIKE '%{term}%'
            ").CountAsync();
        }
    }
}