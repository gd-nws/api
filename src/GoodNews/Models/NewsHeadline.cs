using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodNews.Models
{
    [Table("headlines")]
    public class NewsHeadline
    {
        [Key]
        [Column("id")]
        public uint Id { get; set; }
        
        [Column("predicted_class")]
        public int? PredictedClass { get; set; }
        
        [Column("headline")]
        public string Headline { get; set; }
        
        [Column("link")]
        public string Link { get; set; }
        
        [Column("origin")]
        public string Origin { get; set; }
        
        [Column("semantic_value")]
        public float SemanticValue { get; set; }
        
        [Column("hashcode")]
        public string Hashcode { get; set; }
        
        [Column("pos")]
        public float Pos { get; set; }
        
        [Column("neg")]
        public float Neg { get; set; }
        
        [Column("neu")]
        public float Nue { get; set; }
        
        [Column("published_at")]
        public DateTime PublishedAt { get; set; }
        
        [Column("display_image")]
        public string DisplayImage { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}