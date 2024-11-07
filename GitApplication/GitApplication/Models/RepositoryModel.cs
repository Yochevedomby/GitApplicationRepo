namespace GitApplication.Models
{
    public class RepositoryModel
    {
        public string Name { get; set; } //שם המאגר
        public string OwnerAvatarUrl { get; set; } // כתובת התמונה של בעל המאגר
        public string GitUrl { get; set; }// קישור ל GitHub של המאגר

    }
}
