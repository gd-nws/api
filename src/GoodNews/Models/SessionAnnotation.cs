using System.ComponentModel.DataAnnotations.Schema;

namespace GoodNews.Models
{
    public class SessionAnnotation
    {
        [Column("headline_id")] public uint HeadlineId { get; set; }

        [Column("vote")] public int Vote { get; set; }
    }
}