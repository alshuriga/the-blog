import { Component, OnInit } from '@angular/core';
import { FormControl, FormControlName, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CreatePostDTO, UpdatePostDTO } from 'src/app/models/PostModels';
import { BlogService } from 'src/app/services/blog.service';
import { Observable } from 'rxjs';
import {  } from '@angular/common/http';
import { ServerValidationService } from 'src/app/services/server-validation.service';
import { faL } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-post-form',
  templateUrl: './post-form.component.html',
  styleUrls: ['./post-form.component.css']
})
export class PostFormComponent implements OnInit {
  validErrors$: Observable<any> = this.validation.serverErrors$;
  private edit: boolean;
  buttonState: boolean = true;

  postForm = new FormGroup({
    'id': new FormControl(),
    'header': new FormControl(''),
    'text': new FormControl(''),
    'isDraft': new FormControl(false),
    'tagString': new FormControl('')
  });

  constructor(private blog: BlogService, private router: Router, private validation: ServerValidationService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.validation.removeServerErrors();  
    this.edit = this.route.snapshot.url[0].path === 'edit';
    if(this.edit)
    {
      const data = this.getPostForUpdate();
      if(data) this.postForm.setValue(data);
    }
  }

  onSubmit() {
   this.buttonState = false;
   if(this.edit) this.updatePost();
   else this.createPost();
  // this.postForm.reset();
   this.buttonState = true;
  }

  getPostForUpdate(): UpdatePostDTO | null {
    const post = localStorage.getItem('post-update');
    if(post) {
      const dto: UpdatePostDTO = JSON.parse(post);
      return dto;
    }
    return null;
  }

  private createPost() {
    let postData: CreatePostDTO = {
      header: this.postForm.value.header!,
      text: this.postForm.value.text!,
      isDraft: this.postForm.value.isDraft ? true : false,
      tagString: this.postForm.value.tagString!
    }
    this.blog.createPost(postData).subscribe(res => this.router.navigate([`post/${res}`]));
  }

  private updatePost() {
    let postData: UpdatePostDTO = {
      id: this.postForm.value.id!,
      header: this.postForm.value.header!,
      text: this.postForm.value.text!,
      isDraft: this.postForm.value.isDraft ? true : false,
      tagString: this.postForm.value.tagString!
    }
    this.blog.updatePost(postData).subscribe(res => this.router.navigate([`post/${postData.id}`]));
  }

}
