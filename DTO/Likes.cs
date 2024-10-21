using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Likes
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // MongoDB ObjectId як string
        public string PostId { get; set; } // Ідентифікатор поста
        public string CommentId { get; set; } // Ідентифікатор коментаря (може бути null)
        public string UserId { get; set; } // Ідентифікатор користувача, який поставив лайк
        public DateTime CreatedAt { get; set; }
    }
}
