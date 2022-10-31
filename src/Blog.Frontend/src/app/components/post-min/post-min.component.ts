import { Component, OnInit, Input } from '@angular/core';
import { PostList } from 'src/app/models/PostModels';
import { faCommentDots } from '@fortawesome/free-regular-svg-icons';
import { Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-post-min',
  templateUrl: './post-min.component.html',
  styleUrls: ['./post-min.component.css']
})
export class PostMinComponent implements OnInit {
  
  faCommentDots = faCommentDots;

  @Input() post: PostList | undefined;
  @Output() byTag = new EventEmitter<string>();
  @Output() onDelete = new EventEmitter();
  
  constructor() { }

  ngOnInit(): void {
  }

  clickTag(tagName: string) {
    this.byTag.emit(tagName);
  }

  clickDelete() {
    this.onDelete.emit();
  }
}
