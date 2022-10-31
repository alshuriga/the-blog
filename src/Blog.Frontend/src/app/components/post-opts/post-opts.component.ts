import { JsonPipe } from '@angular/common';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { BlogService } from '../../services/blog.service';

@Component({
  selector: 'app-post-opts',
  templateUrl: './post-opts.component.html',
  styleUrls: ['./post-opts.component.css']
})
export class PostOptsComponent implements OnInit {

  @Input() postId: number;
  @Output() onDelete = new EventEmitter();
  @Output() onEdit = new EventEmitter();

  constructor(private blog: BlogService, private router: Router) { }

  ngOnInit(): void {
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
