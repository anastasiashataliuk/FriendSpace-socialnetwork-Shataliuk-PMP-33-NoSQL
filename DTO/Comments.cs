using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DTO
{
    public class Comments
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // MongoDB ObjectId як string
        public string PostId { get; set; } // Ідентифікатор поста, до якого коментують
        public string UserId { get; set; } // Ідентифікатор користувача (Email або Id)
        public string Content { get; set; } // Зміст коментаря
        public DateTime CreatedAt { get; set; } // Дата створення коментаря
        public List<string> Likes { get; set; }
    }
}
