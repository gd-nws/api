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
                SELECT *
			    FROM headlines h
			    WHERE 
			      h.predicted_class {op} 0
                AND DATE(h.published_at) = CURDATE() - INTERVAL {dateOffset} DAY
                ORDER BY h.semantic_value {sort}
                LIMIT {limit}
                OFFSET {offset}
            ").ToListAsync();
        }

        /// <summary>
        /// Fetch a single headline.
        /// </summary>
        /// <param name="headlineId"></param>
        /// <returns></returns>
        public async Task<NewsHeadline> GetHeadline(int headlineId)
        {
            var result = await Db.NewsHeadlines.FromSqlRaw($@"
                SELECT *
                FROM headlines h
                WHERE h.id = {headlineId}
            ").ToListAsync();

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
            ").CountAsync();
        }
    }
}