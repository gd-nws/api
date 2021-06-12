using MongoDB.Bson.Serialization.Attributes;

namespace GoodNews.Models.DBModels.Mongo
{
    public class HeadlineVotes : IHeadlineVotes
    {
        [BsonElement("positive")]
        public int Positive { get; set; }

        [BsonElement("negative")]
        public int Negative { get; set; }
    }
}
