import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { BlogService } from '../../services/blog.service';
import { AuthService } from 'src/app/services/auth.service';
import { Observable } from 'rxjs';
import { UserClaims } from 'src/app/models/UserModels';

@Component({
  selector: 'app-post-opts',
  templateUrl: './post-opts.component.html',
  styleUrls: ['./post-opts.component.css']
})
export class PostOptsComponent implements OnInit {

  @Input() postId: number;
  @Output() onDelete = new EventEmitter();
  @Output() onEdit = new EventEmitter();
  isAdmin: boolean = false;
  constructor(private blog: BlogService, private router: Router, private _auth: AuthService) { }

  ngOnInit(): void {
    this._auth.claims.subscribe((res) => this.isAdmin = res ? res.role.includes("Admins") : false);
  }

  onClickDelete() {
    this.blog.deletePost(this.postId).subscribe(() => {
      this.onDelete.emit();
    })
  }

  onClickEdit() {
    this.blog.getEditPost(this.postId).subscribe((res) => {
      localStorage.setItem('post-update', JSON.stringify(res));
      this.router.navigate(['edit']);
    });
    
  }
}
