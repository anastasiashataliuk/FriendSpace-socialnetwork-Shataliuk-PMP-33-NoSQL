using DAL1.Interface;
using DTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL1.Concreate
{
    public class PostsDAL : IPostsDAL
    {
        private readonly IMongoCollection<Posts> _postsCollection;

        public PostsDAL(IMongoDatabase database)
        {
            _postsCollection = database.GetCollection<Posts>("Posts"); // Назва колекції
        }

        // Додати новий пост
        public void AddPost(Posts post)
        {
            _postsCollection.InsertOne(post);
        }

        // Отримати пост за ID
        public Posts GetPostById(ObjectId id)
        {
            var filter = Builders<Posts>.Filter.Eq(p => p.Id, id);
            return _postsCollection.Find(filter).FirstOrDefault();
        }

        // Отримати всі пости
        public List<Posts> GetAllPosts()
        {
            return _postsCollection.Find(new BsonDocument()).ToList();
        }

        // Оновити пост
        public void UpdatePost(ObjectId id, Posts updatedPost)
        {
            var filter = Builders<Posts>.Filter.Eq(p => p.Id, id);
            _postsCollection.ReplaceOne(filter, updatedPost);
        }

        // Видалити пост
        public void DeletePost(ObjectId id)
        {
            var filter = Builders<Posts>.Filter.Eq(p => p.Id, id);
            _postsCollection.DeleteOne(filter);
        }

        // Отримати пости за ID користувача
        public List<Posts> GetPostsByUserId(string userId)
        {
            var filter = Builders<Posts>.Filter.Eq(p => p.UserId, userId);
            return _postsCollection.Find(filter).ToList();
        }

        // Додати лайк до поста
        public void AddLike(ObjectId postId)
        {
            var filter = Builders<Posts>.Filter.Eq(p => p.Id, postId);
            var update = Builders<Posts>.Update.AddToSet(p => p.Likes, postId); 

            _postsCollection.UpdateOne(filter, update);
        }

    }
}
