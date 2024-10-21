using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace DTO
{
    public class Users
    {
        [BsonId]
        public ObjectId Id { get; set; } // Change from string to ObjectId
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public List<string> Interests { get; set; }
        public List<ObjectId> Friends { get; set; } = new();
    }
}
