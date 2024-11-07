import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from './components/search/search.component';
import { BookmarksComponent } from './components/bookmarks/bookmarks.component';
import { AuthGuard } from './core/auth.guard';

const routes: Routes = [
  { path: 'search', component: SearchComponent },
  { path: 'bookmarks', component: BookmarksComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/search', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
