using DTO;
using MongoDB.Bson;
using System.Collections.Generic;

namespace DAL1.Interface
{
    public interface IPostsDAL
    {
        void AddPost(Posts post);
        Posts GetPostById(ObjectId id);
        List<Posts> GetAllPosts();
        void UpdatePost(ObjectId id, Posts updatedPost);
        void DeletePost(ObjectId id);
        List<Posts> GetPostsByUserId(string userId);

        void AddLike(ObjectId postId);
    }
}
