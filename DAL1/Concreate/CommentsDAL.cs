using DAL1.Interface;
using DTO;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL1.Concrete
{
    public class CommentsDAL : ICommentsDAL
    {
        private readonly IMongoCollection<Comments> _commentsCollection;

        public CommentsDAL(IMongoDatabase database)
        {
            _commentsCollection = database.GetCollection<Comments>("Comments"); // Назва колекції
        }

        // Додати новий коментар
        public void AddComment(Comments comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            _commentsCollection.InsertOne(comment);
        }

        // Отримати коментар за ID
        public Comments GetCommentById(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("ID cannot be null or empty.", nameof(id));
            var filter = Builders<Comments>.Filter.Eq(c => c.Id, id);
            return _commentsCollection.Find(filter).FirstOrDefault();
        }

        // Отримати всі коментарі
        public List<Comments> GetAllComments()
        {
            return _commentsCollection.Find(new BsonDocument()).ToList();
        }

        // Оновити коментар
        public void UpdateComment(string id, Comments updatedComment)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("ID cannot be null or empty.", nameof(id));
            if (updatedComment == null) throw new ArgumentNullException(nameof(updatedComment));

            var filter = Builders<Comments>.Filter.Eq(c => c.Id, id);
            _commentsCollection.ReplaceOne(filter, updatedComment);
        }

        // Видалити коментар
        public void DeleteComment(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("ID cannot be null or empty.", nameof(id));
            var filter = Builders<Comments>.Filter.Eq(c => c.Id, id);
            _commentsCollection.DeleteOne(filter);
        }

        // Отримати коментарі за ID поста
        public List<Comments> GetCommentsByPostId(string postId)
        {
            if (string.IsNullOrWhiteSpace(postId)) throw new ArgumentException("Post ID cannot be null or empty.", nameof(postId));
            var filter = Builders<Comments>.Filter.Eq(c => c.PostId, postId);
            return _commentsCollection.Find(filter).ToList();
        }
    }
}
