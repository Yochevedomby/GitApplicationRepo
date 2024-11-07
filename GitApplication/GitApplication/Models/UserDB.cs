namespace GitApplication.Models
{
    public class UserDB
    {
        // רשימת משתמשים מדומה עם שם משתמש וסיסמה
        public static List<UserSession> Users = new List<UserSession>
        {
            new UserSession { Username = "user1", Password = "1234", BookmarkedRepositories = new List<RepositoryModel>() },
            new UserSession { Username = "user2", Password = "7789", BookmarkedRepositories = new List<RepositoryModel>() }
        };

        public static UserSession Authenticate(string username, string password)
        {
            // מחפש משתמש ברשימה לפי שם משתמש וסיסמה
            return Users.FirstOrDefault(user => user.Username == username && user.Password == password);
        }
    }
}
