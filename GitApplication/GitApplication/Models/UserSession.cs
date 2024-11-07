namespace GitApplication.Models
{
    public class UserSession
    {
        public string Username { get; set; }
        public string Password { get; set; }    
        // רשימת הסימניות שנשמרו על ידי המשתמש
        public List<RepositoryModel> BookmarkedRepositories { get; set; } = new List<RepositoryModel>();
    }
}
