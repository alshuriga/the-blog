import { Component, OnInit, Input } from '@angular/core';
import { faHeart as notLikedIcon } from '@fortawesome/free-regular-svg-icons';
import { faHeart as likedIcon } from '@fortawesome/free-solid-svg-icons';
import { PostDTO, PostList } from '../models/PostModels';
import { AuthService } from '../services/auth.service';
import { BlogService } from '../services/blog.service';
import { UserClaims } from '../models/UserModels';

@Component({
  selector: 'app-like-button',
  templateUrl: './like-button.component.html',
  styleUrls: ['./like-button.component.css']
})
export class LikeButtonComponent implements OnInit {

  claims: UserClaims | undefined;
  isLikedbyCurrentUser: boolean;
  likesNumber: number;
  isLoggedIn: boolean;
  @Input() post: PostDTO;
  likedIcon = likedIcon;
  notLikedIcon = notLikedIcon;

  constructor(private auth: AuthService, private blog: BlogService) { }

  ngOnInit(): void {
    this.likesNumber = this.post.likes.length
    this.auth.claims.subscribe((cl) => {
      this.claims = cl;
      this.isLikedbyCurrentUser = this.post.likes.some(l => l.username == this.claims?.username);
    });
    this.auth.isLoggedIn.subscribe((res) => this.isLoggedIn = res);
  }

  onClick(): void {
    if (this.isLikedbyCurrentUser)
      this.blog.unLikePost(this.post.id).subscribe(() => this.likesNumber -= 1);
    else
      this.blog.likePost(this.post.id).subscribe(() => this.likesNumber += 1);

    this.isLikedbyCurrentUser = !this.isLikedbyCurrentUser;
  }

  private updatePost(isLiked:boolean): void {
    this.blog.getPost(this.post.id).subscribe(p => {
      this.likesNumber = p.post.likes.length;
    });
  }

}
