using MongoDB.Bson;
using MongoDB.Driver;
using DAL1.Interface;
using DTO;
using System.Collections.Generic;

namespace DAL1.Concreate
{
    public class UserDAL : IUserDAL
    {
        private readonly IMongoCollection<Users> _usersCollection;

        public UserDAL(IMongoDatabase database)
        {
            _usersCollection = database.GetCollection<Users>("Users"); // Назва колекції
        }

        // Додати нового користувача
        public void AddUser(Users user)
        {
            _usersCollection.InsertOne(user);
        }

        // Отримати користувача за ID
        public Users GetUserById(ObjectId id)
        {
            var filter = Builders<Users>.Filter.Eq(u => u.Id, id);
            return _usersCollection.Find(filter).FirstOrDefault();
        }

        // Отримати користувача за Email
        public Users GetUserByEmail(string email)
        {
            var filter = Builders<Users>.Filter.Eq(u => u.Email, email);
            return _usersCollection.Find(filter).FirstOrDefault();
        }

        // Оновити користувача
        public void UpdateUser(ObjectId id, Users updatedUser)
        {
            var filter = Builders<Users>.Filter.Eq(u => u.Id, id);
            _usersCollection.ReplaceOne(filter, updatedUser);
        }

        // Видалити користувача
        public void DeleteUser(ObjectId id)
        {
            var filter = Builders<Users>.Filter.Eq(u => u.Id, id);
            _usersCollection.DeleteOne(filter);
        }

        // Отримати всіх користувачів
        public List<Users> GetAllUsers()
        {
            return _usersCollection.Find(new BsonDocument()).ToList();
        }

        public List<Users> SearchUsersByUsername(string username)
        {
            var filter = Builders<Users>.Filter.Regex(u => u.Username, new BsonRegularExpression(username, "i")); // Case insensitive
            return _usersCollection.Find(filter).ToList();
        }

        public void AddFriend(ObjectId userId, ObjectId friendId)
        {
            var filter = Builders<Users>.Filter.Eq(u => u.Id, userId);
            var update = Builders<Users>.Update.AddToSet(u => u.Friends, friendId); // Assuming you have a `Friends` list of type List<ObjectId>
            _usersCollection.UpdateOne(filter, update);
        }


    }
}
