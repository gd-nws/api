using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNews.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodNews.Repositories.Postgres
{
    public class AnnotationRepository : PostgresRepository, IAnnotationRepository
    {
        public AnnotationRepository(GoodNewsDBContext db) : base(db)
        {
        }

        public async Task<List<SessionAnnotation>> GetSessionAnnotations(string session)
        {
            var result = await Db.SessionAnnotations.FromSqlRaw($@"
                SELECT *
                FROM good_news.session_annotations sa
                WHERE sa.session_id = '{session}'
                ORDER BY sa.created_at DESC
            ").ToListAsync();

            return result;
        }

        public async Task CreateSessionAnnotation(NewsHeadline headline, Session session, HeadlineSentiment sentiment)
        {
            int positive;
            int negative;
            int vote;
            switch (sentiment)
            {
                case HeadlineSentiment.POSITIVE:
                    positive = 1;
                    negative = 0;
                    vote = 1;
                    break;
                case HeadlineSentiment.NEGATIVE:
                    positive = 0;
                    negative = 1;
                    vote = -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(sentiment), sentiment, null);
            }

            await using var trx = await Db.Database.BeginTransactionAsync();
            await Db.Database.ExecuteSqlRawAsync($@"
                    INSERT INTO good_news.annotations
                        (headline_id, positive, negative)
                    VALUES ({headline.Id}, {positive}, {negative})
                ");

            await Db.Database.ExecuteSqlRawAsync($@"
                    INSERT INTO good_news.session_annotations
                        (session_id, headline_id, vote)
                    VALUES ('{session.Uuid}', {headline.Id}, {vote})
                ");

            await trx.CommitAsync();
        }
    }
}