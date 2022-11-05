import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccessDeniedComponent } from './components/access-denied/access-denied.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { PostFormComponent } from './components/post-form/post-form.component';
import { PostsPageComponent } from './components/posts-page/posts-page.component';
import { SignInFormComponent } from './components/sign-in-form/sign-in-form.component';
import { SignUpFormComponent } from './components/sign-up-form/sign-up-form.component';
import { SinglePostComponent } from './components/single-post/single-post.component';

const routes: Routes = [
  { path: 'list', component: PostsPageComponent },
  { path: 'drafts', component: PostsPageComponent },
  { path: '', redirectTo: 'list', pathMatch: 'full' },
  { path: 'post/:id', component: SinglePostComponent },
  { path: 'signin', component: SignInFormComponent },
  { path: 'edit', component: PostFormComponent },
  { path: 'new', component: PostFormComponent },
  { path: 'access-denied', component: AccessDeniedComponent },
  { path: 'admin-panel', component: AdminPanelComponent },
  { path: 'signup', component: SignUpFormComponent },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
