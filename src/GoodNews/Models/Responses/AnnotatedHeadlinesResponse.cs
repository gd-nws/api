using System.Collections.Generic;

namespace GoodNews.Models.Responses
{
  public class AnnotatedHeadlinesResponse
  {
    public IList<AnnotatedHeadline> Headlines { get; set; }
    public int Count { get; set; }
  }
}