// src/app/features/bookmarks/bookmarks.component.ts
import { Component, OnInit } from '@angular/core';
import { GithubService } from 'src/app/core/services/github.service';

@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.component.html',
  styleUrls: ['./bookmarks.component.scss'],
})
export class BookmarksComponent implements OnInit {
  bookmarks: any[] = [];

  constructor(private githubService: GithubService) {}

  ngOnInit() {
    this.loadBookmarks();
  }

  loadBookmarks() {
    this.githubService.getBookmarks().subscribe((data: any) => {
      this.bookmarks = data.items;
    });

    
  }
}
