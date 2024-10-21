using DTO;

namespace DAL1.Interface
{
    public interface ICommentsDAL
    {
        void AddComment(Comments comment);
        Comments GetCommentById(string id);
        List<Comments> GetAllComments();
        void UpdateComment(string id, Comments updatedComment);
        void DeleteComment(string id);
        List<Comments> GetCommentsByPostId(string postId);
    }
}
