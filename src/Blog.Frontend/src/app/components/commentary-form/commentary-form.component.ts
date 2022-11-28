import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { faThinkPeaks } from '@fortawesome/free-brands-svg-icons';
import { Observable, Observer } from 'rxjs';
import { BlogService } from 'src/app/services/blog.service';
import { ServerValidationService } from 'src/app/services/server-validation.service';

@Component({
  selector: 'app-commentary-form',
  templateUrl: './commentary-form.component.html',
  styleUrls: ['./commentary-form.component.css']
})
export class CommentaryFormComponent implements OnInit {
  @Input() postId: number;
  @Output() addComment = new EventEmitter();

  buttonState: boolean = true;

  validErrors$: Observable<any> = this.validation.serverErrors$;

  commentForm = new FormGroup({
    text: new FormControl(),
    postId: new FormControl()
  })
  constructor(private blog: BlogService, private validation: ServerValidationService) { }

  ngOnInit(): void {
    this.validation.removeServerErrors();
    this.commentForm.value.postId = this.postId;
  }

  onSubmit() {
    const obs = {
      next: () => {
        this.addComment.emit();
        this.commentForm.reset(); 
      },
      complete: () => {
        this.buttonState = true;
      }
    }
    this.buttonState = false;
    console.log(this.postId.toString());
    console.log(this.commentForm.value.text);
    this.blog.createCommentary(this.commentForm.value.text, this.postId).subscribe(obs);
  }

}
