using GitApplication.Models;
using System.Text.Json;

namespace GitApplication.Services
{
    //שרות המבצע בקשות חיפוש למאגרים ב GitHub API
    public class GitHubService
    {
        private readonly HttpClient _httpClient;

        public GitHubService(HttpClient httpClient)
        {
            _httpClient = httpClient;//לשרות httpClient מזריקים 

        }
        public async Task<List<RepositoryModel>> SearchRepositoriesAsync(string keyWord)
        {
            //עם מילת החיפוש מהמשתמש APIמבצעים בקשה ל
            var response = await _httpClient.GetAsync($"https://api.github.com/search/repositories?q={keyWord}");
            response.EnsureSuccessStatusCode();
            //המרת התגובה לאובייקט שלי
            var responseBody = await response.Content.ReadAsStringAsync();
            var searchResult = JsonSerializer.Deserialize<GitHubSearchResult>(responseBody);

            return searchResult?.Items ?? new List<RepositoryModel>();//יוחזר מערך ריק NULL אם זה  

        }
    }
    // מודל למיפוי תוצאות חיפוש GitHub
    public class GitHubSearchResult
    {
        public List<RepositoryModel> Items { get; set; }
    }
}
