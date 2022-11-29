import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PostMinComponent } from './components/post-min/post-min.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { PostsPageComponent } from './components/posts-page/posts-page.component';
import { PaginatorComponent } from './components/paginator/paginator.component';
import { SinglePostComponent } from './components/single-post/single-post.component';
import { CommentaryFormComponent } from './components/commentary-form/commentary-form.component';
import { SignInFormComponent } from './components/sign-in-form/sign-in-form.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { UserPanelComponent } from './components/user-panel/user-panel.component';
import { PostFormComponent } from './components/post-form/post-form.component';
import { ServerErrorsInterceptor } from './interceptors/server-errors.interceptor';
import { AccessDeniedComponent } from './components/access-denied/access-denied.component';
import { AdminPanelComponent } from './components/admin-panel/admin-panel.component';
import { PostOptsComponent } from './components/post-opts/post-opts.component';
import { SignUpFormComponent } from './components/sign-up-form/sign-up-form.component';
import { NotFoundComponent } from './not-found/not-found.component';


@NgModule({
  declarations: [
    AppComponent,
    PostMinComponent,
    NavBarComponent,
    PostsPageComponent,
    PaginatorComponent,
    SinglePostComponent,
    CommentaryFormComponent,
    SignInFormComponent,
    UserPanelComponent,
    PostFormComponent,
    AccessDeniedComponent,
    AdminPanelComponent,
    PostOptsComponent,
    SignUpFormComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FontAwesomeModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: ServerErrorsInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
