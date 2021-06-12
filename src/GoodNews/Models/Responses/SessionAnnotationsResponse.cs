using System.Collections.Generic;
using GoodNews.Models.DBModels.Postgres;

namespace GoodNews.Models.Responses
{
  public class SessionAnnotationsResponse
  {
    public IList<SessionAnnotation> Annotations { get; set; }
  }
}