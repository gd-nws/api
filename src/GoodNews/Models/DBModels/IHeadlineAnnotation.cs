using System;

namespace GoodNews.Models.DBModels
{
    public interface IHeadlineAnnotation
    {
        int Vote { get; set; }
        string SessionId { get; set; }
        DateTime CreatedAt { get; set; }
    }
}