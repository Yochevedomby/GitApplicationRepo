import { ChangeDetectorRef, Component } from '@angular/core';
import { GithubService } from 'src/app/core/services/github.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {
  query: string = '';
  repositories: any[] = [];
  isLoginModalOpen = false; // מצב אם המודאל פתוח או לא
  currentUser: any;

  constructor(private githubService: GithubService, private authService: AuthService,private router: Router,private cdr: ChangeDetectorRef) {}

  ngOnInit(){
    this.currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
  }
  search(): void {
    this.githubService.searchRepositories(this.query).subscribe((data: any) => {
      this.repositories = data.items;
    });
  }

  bookmark(repo: any) {
    this.githubService.addBookmark(repo).subscribe(() => {
      console.log('Repository bookmarked successfully');
    });
  }

  goToBookmarks() {
    this.router.navigate(['/bookmarks']);  // משמעת שהנתיב לקומפוננטה הוא '/bookmarks'
  }

  // פונקציה לפתיחת המודאל
  openLoginModal() {
    this.isLoginModalOpen = true;
 
  }
  // פונקציה לסגירת המודאל
  closeLoginModal() {
    this.isLoginModalOpen = false;
 
  }
  logout(){
    this.authService.logout();
  }
}
