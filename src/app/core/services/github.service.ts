
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RepositoryModel } from 'src/app/models/repository.model';

@Injectable({
  providedIn: 'root',
})
export class GithubService {
  private apiUrl = "https://localhost:7183/api/Repositories";

  constructor(private http: HttpClient) {}

  searchRepositories(query: string): Observable<any> {
    var url = `${this.apiUrl}/search/${query}`
    return this.http.get(url);
  }

  addBookmark(repository: RepositoryModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/bookmark`, repository);
  }

  removeBookmark(repository: RepositoryModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/remove`, repository);
  }

  getBookmarks(): Observable<RepositoryModel[]> {
    return this.http.get<any[]>(`${this.apiUrl}/bookmarks`);
  }
}
