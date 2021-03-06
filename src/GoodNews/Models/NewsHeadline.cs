using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodNews.Models
{
    [Table("headlines")]
    public class NewsHeadline
    {
        [Key] [Column("id")] public long Id { get; set; }

        [Column("predicted_class")] public long? PredictedClass { get; set; }

        [Column("headline")] public string Headline { get; set; }

        [Column("link")] public string Link { get; set; }

        [Column("origin")] public string Origin { get; set; }

        [Column("semantic_value")] public double SemanticValue { get; set; }

        [Column("hashcode")] public string Hashcode { get; set; }

        [Column("pos")] public double Pos { get; set; }

        [Column("neg")] public double Neg { get; set; }

        [Column("neu")] public double Nue { get; set; }

        [Column("published_at")] public DateTime PublishedAt { get; set; }

        [Column("display_image")] public string DisplayImage { get; set; }

        [Column("created_at")] public DateTime CreatedAt { get; set; }

        [Column("positive_votes")] public int PositiveVotes { get; set; }

        [Column("negative_votes")] public int NegativeVotes { get; set; }
    }
}