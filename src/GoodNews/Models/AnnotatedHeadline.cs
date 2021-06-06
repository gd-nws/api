namespace GoodNews.Models
{
  public class AnnotatedHeadline : NewsHeadline
  {
    public int PositiveVotes { get; set; }
    public int NegativeVotes { get; set; }
  }
}