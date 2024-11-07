using GitApplication.Models;
using System.Text.Json;

namespace GitApplication.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

         private ISession Session => _httpContextAccessor.HttpContext.Session;

        // הוספת סימניה לסשן של המשתמש
        public void AddBookmark(RepositoryModel repository)
        {
            try
            {
                UserSession user = new UserSession();

                user.BookmarkedRepositories.Add(repository);
                
                //SaveUserSession(userSession);
                //Session.SetString("UserSession", JsonSerializer.Serialize(userSession));
            }
            catch (JsonException ex)
            {
                // טיפול בשגיאת ניתוח JSON אם קורה משהו לא צפוי
                Console.WriteLine($"Error serializing session data: {ex.Message}");
            }
        }

        public void RemoveBookmark(RepositoryModel bookmark)
        {
            UserSession user = new UserSession();
            if (user != null && user.BookmarkedRepositories.Contains(bookmark))
            {
                user.BookmarkedRepositories.Remove(bookmark);
                //SaveUserSession(userSession);  // שומר את הנתונים מחדש בסשן
            }
        }

        // שליפת הסימניות מהסשן
        public List<RepositoryModel> GetBookmarks()
        {
            try
            {
                UserSession user = new UserSession();
                return user?.BookmarkedRepositories ?? new List<RepositoryModel>();
            }
            catch (JsonException ex)
            {
                // טיפול בשגיאה אם אין אפשרות לפענח את הסשן
                Console.WriteLine($"Error deserializing session data: {ex.Message}");
                return new List<RepositoryModel>(); // החזרת רשימה ריקה במקרה של שגיאה
            }
        }




        
    }
}
