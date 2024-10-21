using DTO;
using MongoDB.Bson;

namespace DAL1.Interface
{
    public interface IUserDAL
    {
        void AddUser(Users user);
        Users GetUserById(ObjectId id);
        Users GetUserByEmail(string email);
        void UpdateUser(ObjectId id, Users updatedUser);
        void DeleteUser(ObjectId id);
        List<Users> GetAllUsers();
        List<Users> SearchUsersByUsername(string username);
        void AddFriend(ObjectId userId, ObjectId friendId);
    }
}
