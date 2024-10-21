using MongoDB.Bson;
using MongoDB.Driver;
using DTO;
using DAL1;

class Program
{
    private static IMongoDatabase _database;
    private static Users _currentUser;

    static void Main(string[] args)
    {
        // Налаштування MongoDB
        var client = new MongoClient("mongodb://localhost:27017/"); // Задайте адресу вашої MongoDB
        _database = client.GetDatabase("FriendSpace"); // Назва вашої бази даних

        // Логування
        _currentUser = Login();
        if (_currentUser != null)
        {
            // Меню для вибору дій
            Menu();
        }
    }

    private static Users Login()
    {
        Console.Write("Enter your email: ");
        var email = Console.ReadLine();
        Console.Write("Enter your password: ");
        var password = Console.ReadLine();

        var userCollection = _database.GetCollection<Users>("Users");
        var user = userCollection.Find(u => u.Email == email && u.Password == password).FirstOrDefault();

        if (user != null)
        {
            Console.WriteLine($"Welcome, {user.Username}!");
            return user;
        }
        else
        {
            Console.WriteLine("Invalid email or password.");
            return null;
        }
    }

    private static void Menu()
    {
        while (true)
        {
            Console.WriteLine("\nFriendSpace:");
            Console.WriteLine("1. View News Feed");
            Console.WriteLine("2. Search and Add Friends");
            Console.WriteLine("3. Remove Friend");
            Console.WriteLine("4. Add Post");
            Console.WriteLine("5. View User Posts");
            Console.WriteLine("6. Exit");
            Console.Write("CHOOSE ACTION: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    DisplayNewsFeed();
                    break;
                case "2":
                    SearchAndAddFriends();
                    break;
                case "3":
                    RemoveFriend();
                    break;
                case "4":
                    AddPost();
                    break;
                case "5":
                    ViewUserPosts();
                    break;
                case "6":
                    return; // Вихід з програми
                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }

    private static void DisplayNewsFeed()
    {
        var postCollection = _database.GetCollection<Posts>("Posts");
        var allPosts = postCollection.Find(post => true).SortByDescending(post => post.CreatedAt).ToList();

        Console.WriteLine("News Feed:");
        foreach (var post in allPosts)
        {
            Console.WriteLine($"- {post.Content} by {post.UserId} on {post.CreatedAt}");
            // Можливість коментувати та ставити лайки
            Console.Write("Do you want to comment or like this post? (comment/like/none): ");
            var action = Console.ReadLine();

            if (action.ToLower() == "comment")
            {
                string postId = post.Id.ToString();
                AddCommentToPost(postId);
            }
            else if (action.ToLower() == "like")
            {
                AddRemoveLike(post.Id.ToString());
            }
        }
    }

    private static void SearchAndAddFriends()
    {
        Console.Write("Enter username or email to search: ");
        var searchQuery = Console.ReadLine();

        var userCollection = _database.GetCollection<Users>("Users");
        var foundUsers = userCollection.Find(u => u.Username.Contains(searchQuery) || u.Email.Contains(searchQuery)).ToList();

        Console.WriteLine("Found users:");
        foreach (var user in foundUsers)
        {
            Console.WriteLine($"- {user.Username} ({user.Email})");
        }

        Console.Write("Enter email of the user to add as a friend: ");
        var emailToAdd = Console.ReadLine();

        var userToAdd = foundUsers.FirstOrDefault(u => u.Email == emailToAdd);
        if (userToAdd != null && !_currentUser.Friends.Contains(userToAdd.Id))
        {
            _currentUser.Friends.Add(userToAdd.Id);
            var update = Builders<Users>.Update.Set(u => u.Friends, _currentUser.Friends);
            userCollection.UpdateOne(u => u.Email == _currentUser.Email, update);
            Console.WriteLine($"Added {userToAdd.Username} as a friend.");
        }
        else
        {
            Console.WriteLine("User not found or already a friend.");
        }
    }

    private static void RemoveFriend()
    {
        Console.Write("Enter the email of the friend to remove: ");
        var emailToRemove = Console.ReadLine();

        var userCollection = _database.GetCollection<Users>("Users");

        // Find the user by email
        var friendToRemove = userCollection.Find(u => u.Email == emailToRemove).FirstOrDefault();

        if (friendToRemove == null)
        {
            Console.WriteLine("User with this email not found.");
            return;
        }

        // Check if the friend exists in the current user's friends list
        if (_currentUser.Friends.Remove(friendToRemove.Id))
        {
            // Update the current user's friends list in the database
            var update = Builders<Users>.Update.Set(u => u.Friends, _currentUser.Friends);
            userCollection.UpdateOne(u => u.Email == _currentUser.Email, update);
            Console.WriteLine($"Removed {friendToRemove.Username} from friends.");
        }
        else
        {
            Console.WriteLine("Friend not found in your list.");
        }
    }


    private static void AddPost()
    {
        Console.Write("Write your post: ");
        var content = Console.ReadLine();

        var post = new Posts
        {
            Id = ObjectId.GenerateNewId(),
            UserId = _currentUser.Email,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            Likes = new List<ObjectId>()
        };

        // Додати пост до MongoDB
        var postCollection = _database.GetCollection<Posts>("Posts");
        postCollection.InsertOne(post);
        Console.WriteLine("Post Added.");
    }

    private static void ViewUserPosts()
    {
        Console.Write("Enter the email of the user whose posts you want to view: ");
        var userEmail = Console.ReadLine();

        var postCollection = _database.GetCollection<Posts>("Posts");
        var userPosts = postCollection.Find(p => p.UserId == userEmail).ToList();

        Console.WriteLine($"Posts by {userEmail}:");
        foreach (var post in userPosts)
        {
            Console.WriteLine($"- {post.Content} (Likes: {post.Likes.Count})");
            // Можливість коментувати
            Console.Write("Do you want to comment on this post? (y/n): ");
            var commentChoice = Console.ReadLine();
            if (commentChoice.ToLower() == "y")
            {
                AddCommentToPost(post.Id.ToString());
            }
        }
    }

    private static void AddCommentToPost(string postId)
    {
        Console.Write("Write your comment: ");
        var commentContent = Console.ReadLine();

        var comment = new Comments
        {
            Id = ObjectId.GenerateNewId().ToString(),
            PostId = postId,
            UserId = _currentUser.Email,
            Content = commentContent,
            CreatedAt = DateTime.UtcNow,
            Likes = new List<string>()
        };

        var commentCollection = _database.GetCollection<Comments>("Comments");
        commentCollection.InsertOne(comment);
        Console.WriteLine("Comment added.");
    }

    private static void AddRemoveLike(string postId)
    {
        Console.Write("Do you want to like or unlike this post? (like/unlike): ");
        var action = Console.ReadLine();

        var likeCollection = _database.GetCollection<Likes>("Likes");

        if (action.ToLower() == "like")
        {
            var like = new Likes
            {
                Id = ObjectId.GenerateNewId().ToString(),
                PostId = postId,
                CommentId = null,
                UserId = _currentUser.Email,
                CreatedAt = DateTime.UtcNow
            };

            likeCollection.InsertOne(like);
            Console.WriteLine("Post liked.");
        }
        else if (action.ToLower() == "unlike")
        {
            likeCollection.DeleteOne(l => l.PostId == postId && l.UserId == _currentUser.Email);
            Console.WriteLine("Post unliked.");
        }
    }
}
