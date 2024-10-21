using DAL1.Interface;
using DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DAL1.Concreate
{
    public class LikesDAL : ILikesDAL
    {
        private readonly IMongoCollection<Likes> _likesCollection;

        public LikesDAL(IMongoDatabase database)
        {
            _likesCollection = database.GetCollection<Likes>("Likes"); // Назва колекції
        }

        // Додати новий лайк
        public void AddLike(Likes like)
        {
            _likesCollection.InsertOne(like);
        }

        // Отримати лайк за ID
        public Likes GetLikeById(string id)
        {
            var filter = Builders<Likes>.Filter.Eq(l => l.Id, id);
            return _likesCollection.Find(filter).FirstOrDefault();
        }

        // Отримати всі лайки
        public List<Likes> GetAllLikes()
        {
            return _likesCollection.Find(new BsonDocument()).ToList();
        }

        // Оновити лайк
        public void UpdateLike(string id, Likes updatedLike)
        {
            var filter = Builders<Likes>.Filter.Eq(l => l.Id, id);
            _likesCollection.ReplaceOne(filter, updatedLike);
        }

        // Видалити лайк
        public void DeleteLike(string id)
        {
            var filter = Builders<Likes>.Filter.Eq(l => l.Id, id);
            _likesCollection.DeleteOne(filter);
        }

        // Отримати лайки за ID поста
        public List<Likes> GetLikesByPostId(string postId)
        {
            var filter = Builders<Likes>.Filter.Eq(l => l.PostId, postId);
            return _likesCollection.Find(filter).ToList();
        }

        // Отримати лайки за ID коментаря
        public List<Likes> GetLikesByCommentId(string commentId)
        {
            var filter = Builders<Likes>.Filter.Eq(l => l.CommentId, commentId);
            return _likesCollection.Find(filter).ToList();
        }
    }
}
