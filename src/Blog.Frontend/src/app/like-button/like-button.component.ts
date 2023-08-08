import { Component, OnInit, Input } from '@angular/core';
import {  faHeart as notLikedIcon} from '@fortawesome/free-regular-svg-icons';
import { IconDefinition, faHeart as likedIcon } from '@fortawesome/free-solid-svg-icons'; 
import { PostList } from '../models/PostModels';
import { AuthService } from '../services/auth.service';
import { BlogService } from '../services/blog.service';
import { UserClaims } from '../models/UserModels';
import { Observable, throwError } from 'rxjs';

@Component({
  selector: 'app-like-button',
  templateUrl: './like-button.component.html',
  styleUrls: ['./like-button.component.css']
})
export class LikeButtonComponent implements OnInit {

  claims: UserClaims | undefined;
  isLikedbyCurrentUser: boolean;
  likesNumber : number;
  @Input() post: PostList;
  likedIcon = likedIcon;
  notLikedIcon = notLikedIcon;

  constructor(private auth : AuthService, private blog : BlogService) { }

  ngOnInit(): void {  
    this.likesNumber = this.post.likes.length
        this.auth.claims.subscribe((cl) => {
          this.claims = cl;
          this.isLikedbyCurrentUser = this.post.likes.some(l => l.username == this.claims?.username);
        });    
  }

  onClick(): void {
    const method = this.isLikedbyCurrentUser ? this.blog.unLikePost(this.post.id) : this.blog.likePost(this.post.id);
    method.subscribe(() => {
      this.updatePost();
      this.isLikedbyCurrentUser = !this.isLikedbyCurrentUser;
    })
  }

  private updatePost() : void {
    this.blog.getPost(this.post.id).subscribe(p => {
      this.likesNumber = p.post.likes.length;
    });
  }

}
