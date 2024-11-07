using GitApplication.Models;
using GitApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace GitApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoriesController : ControllerBase
    {
        private readonly GitHubService _gitHubService;
        private readonly SessionService _sessionService;    
        private readonly HttpClient _httpClient;
        public RepositoriesController(GitHubService gitHubService,SessionService sessionService, HttpClient httpClient)
        {
            _gitHubService = gitHubService;
            _sessionService = sessionService;        
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "MyGitApp"); // הוספת ה-User-Agent
        }


        //GITחיפוש מאגרים ב
        [HttpGet("search/{query}")]
        public async Task<IActionResult> SearchRepositories(string query)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query parameter is required");
          
            var url = $"https://api.github.com/search/repositories?q={query}";
            var response = await _httpClient.GetAsync($"https://api.github.com/search/repositories?q={query}");
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(url);
            }

            var data = await response.Content.ReadAsStringAsync();
            return Ok(data); // מחזיר את המידע שנמצא
        }

        // פעולה לשמירת סימניה למאגר
        //[Authorize] // ודא שהמשתמש מחובר
        [HttpPost("bookmark")]
        public IActionResult BookmarkRepository([FromBody] RepositoryModel repository)
        {
            // קבלת מזהה המשתמש מהטוקן
            var userId = User.FindFirst("UserName")?.Value;
            var authorizationHeader = Request.Headers["Authorization"];
            var userNameHeader = Request.Headers["UserName"];
            repository.Name = userNameHeader;
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                _sessionService.AddBookmark(repository);
                return Ok("Bookmark added successfully.");
                
            }

            return Unauthorized("User is not logged in.");
        }

        // פעולה למחיקת סימניה למאגר
        [HttpPost("remove")]
        public IActionResult BookmarkRemove([FromBody] RepositoryModel bookmark)
        {
            
            var authorizationHeader = Request.Headers["Authorization"];
            var userNameHeader = Request.Headers["UserName"];
            bookmark.Name = userNameHeader;
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("No user session found");
            }
            _sessionService.RemoveBookmark(bookmark);
            return Ok("remove");
        }

        // פעולה להצגת כל הסימניות שנשמרו
        [HttpGet("bookmarks")]
        public IActionResult GetBookmarks()
        {
            
            UserSession user = new UserSession();
            
            var authorizationHeader = Request.Headers["Authorization"];
            var userNameHeader = Request.Headers["UserName"];
            user.Username = userNameHeader;
            
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("User is not logged in.");
            }

            return Ok(_sessionService.GetBookmarks());
        }

    }
}
