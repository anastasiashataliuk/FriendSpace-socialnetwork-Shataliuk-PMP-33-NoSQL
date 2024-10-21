using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DTO
{
    public class Posts
    {
        [BsonId]
        public ObjectId Id { get; set; } // MongoDB ObjectId як string
        public string UserId { get; set; } // Ідентифікатор користувача (Email або Id)
        public string Content { get; set; } // Зміст поста
        public DateTime CreatedAt { get; set; } // Дата створення поста
        public List<ObjectId> Likes { get; set; } = new();
    }
}
