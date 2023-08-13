import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { url } from '../webapi-constants';
import { PostSingleVM, PostsPage, CreatePostDTO, UpdatePostDTO } from '../models/PostModels';
import { User, UserSignUpDTO, UsersListVM } from '../models/UserModels';
import { CreateCommenaryDTO } from '../models/CommentaryModels';


@Injectable({
  providedIn: 'root'
})
export class BlogService {
  private url: string = url;
  private options = { headers: new HttpHeaders({ 'Content-Type': 'application/json' })};
  constructor(private http: HttpClient) { }

  getPostsPage(page = 0, isDraft = false, tagName?:string): Observable<PostsPage> {
    let url = `${this.url}Post/List/${page}?isDraft=${isDraft}`;
    if(tagName) url = url.concat(`&tagName=${tagName}`);
    return this.http.get<PostsPage>(url);
  }

  getPost(id: number): Observable<PostSingleVM> {
    let url = `${this.url}Post/${id}`;
    return this.http.get<PostSingleVM>(url);
  }

  signIn(user: User): Observable<any> { 
    let url = `${this.url}Account/login`;
    return this.http.post(url, user, this.options);
  }

  signUp(user: UserSignUpDTO): Observable<any> {
    let url = `${this.url}Account/signup`;
    return this.http.post(url, user, this.options);
  }

  createCommentary(text: string, postId: number): Observable<any> {
    const comment: CreateCommenaryDTO = { text: text, postId: postId};
    let url = `${this.url}Commentary`;
    return this.http.post(url, comment, this.options);
  }

  createPost(post: CreatePostDTO): Observable<number> {
    const url = `${this.url}Post`;
    const id =  this.http.post(url, post, this.options).pipe(map(res => Number.parseInt(res as string)));
    return id;
  }

  getUsersList(): Observable<UsersListVM> {
    let url = `${this.url}Users`;
    return this.http.get<UsersListVM>(url);
  }

  toggleAdmin(id: string): Observable<any> {
    let url = `${this.url}Users/${id}`;
    return this.http.put(url, null, this.options);
  }

  deleteUser(id: string): Observable<any> {
    let url = `${this.url}Users/${id}`;
    return this.http.delete(url, this.options);
  }

  getEditPost(postId: number): Observable<UpdatePostDTO> {
    const url = `${this.url}Post/Edit/${postId}`;
    return this.http.get<UpdatePostDTO>(url);
  }

  updatePost(post: UpdatePostDTO): Observable<any> {
    const url = `${this.url}Post`;
    return this.http.put(url, post, this.options);
  }

  deletePost(postId: number): Observable<any> {
    const url = `${this.url}Post/${postId}`;
    return this.http.delete(url);
  }

  deleteCommentary(commentaryId: number): Observable<any> {
    const url = `${this.url}Commentary/${commentaryId}`;
    return this.http.delete(url);
  }

  likePost(postId: number): Observable<any> {
    const url = `${this.url}Like/${postId}`;
    return this.http.post(url, null);
  }

  unLikePost(postId: number): Observable<any> {
    const url = `${this.url}Like/${postId}`;
    return this.http.delete(url);
  }
}
