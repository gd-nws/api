using System.ComponentModel.DataAnnotations;

namespace GoodNews.Models.Requests
{
    public class NewSessionAnnotationRequest
    {
        [Required] public HeadlineSentiment Annotation { get; set; }

        [Required] public string SessionToken { get; set; }
    }
}