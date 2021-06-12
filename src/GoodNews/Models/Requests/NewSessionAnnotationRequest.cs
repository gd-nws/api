using System.ComponentModel.DataAnnotations;

namespace GoodNews.Models.Requests
{
    public class NewSessionAnnotationRequest
    {
        [Required]
        public HeadlineSentiment Annotation { get; set; }

        [Required]
        [StringLength(24, MinimumLength = 24)]
        public string SessionToken { get; set; }
    }
}