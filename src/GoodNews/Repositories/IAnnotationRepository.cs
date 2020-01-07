using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNews.Models;

namespace GoodNews.Repositories
{
    public interface IAnnotationRepository
    {
        Task<List<SessionAnnotation>> GetSessionAnnotations(string session);
        Task CreateSessionAnnotation(NewsHeadline headline, Session session, HeadlineSentiment sentiment);
    }
}