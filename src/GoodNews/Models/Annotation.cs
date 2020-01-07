using System.ComponentModel.DataAnnotations.Schema;

namespace GoodNews.Models
{
    [Table("annotations")]
    public class Annotation
    {
        [Column("id")]
        public uint Id { get; set; }
        
        [Column("positive")]
        public int Positive { get; set; }
        
        [Column("negative")]
        public int Negative { get; set; }
        
        [Column("uuid")]
        public string Uuid { get; set; }
        
        [ForeignKey("annotations_headlines_id_fk")]
        public NewsHeadline NewsHeadline { get; set; }
    }
}