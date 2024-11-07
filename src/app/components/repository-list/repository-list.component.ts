import { Component, EventEmitter, Input, Output } from '@angular/core';
import { faHeart, faHeartBroken } from '@fortawesome/free-solid-svg-icons';
import { GithubService } from 'src/app/core/services/github.service';
import { RepositoryModel } from 'src/app/models/repository.model';

@Component({
  selector: 'app-repository-list',
  templateUrl: './repository-list.component.html',
  styleUrls: ['./repository-list.component.scss']
})
export class RepositoryListComponent {
  @Input() repositories: any[] = [];
  //@Output() bookmarkRepository: EventEmitter<any> = new EventEmitter<any>();

  constructor(private githubService: GithubService) { }
  faHeart = faHeart;
  faHeartBroken = faHeartBroken;

  // פונקציה שתיקרא כאשר הכפתור נשמע
  // onBookmark(repository: any): void {
  //   this.bookmarkRepository.emit(repository); // שולח את המאגר שהמשתמש בחר לשמירה למועדפים
  // }

  toggleBookmark(repo: any): void {
    repo.isBookmarked = !repo.isBookmarked;
    //this.bookmarkRepository.emit(repo); // שולח את המאגר שהמשתמש בחר לשמירה למועדפים
    const repoBook = new RepositoryModel();
    repoBook.name = repo.name;
    repoBook.ownerAvatarUrl = repo.owner.avatar_url;
    repoBook.gitUrl = repo.owner.html_url;
    if (repo.isBookmarked) {
      this.githubService.addBookmark(repoBook).subscribe(() => {
        console.log('Repository bookmarked successfully');
      });
    }
    else{
      if(!repo.isBookmarked){
        this.githubService.removeBookmark(repoBook).subscribe(() => {
          console.log('Repository bookmarked successfully');
        });
      }
    }
  }

}
