using DTO;
using System.Collections.Generic;

namespace DAL1.Interface
{
    public interface ILikesDAL
    {
        void AddLike(Likes like);
        Likes GetLikeById(string id);
        List<Likes> GetAllLikes();
        void UpdateLike(string id, Likes updatedLike);
        void DeleteLike(string id);
        List<Likes> GetLikesByPostId(string postId);
        List<Likes> GetLikesByCommentId(string commentId);
    }
}
