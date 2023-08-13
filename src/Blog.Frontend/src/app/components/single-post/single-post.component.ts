import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PostSingleVM } from 'src/app/models/PostModels';
import { AuthService } from 'src/app/services/auth.service';
import { BlogService } from 'src/app/services/blog.service';
@Component({
  selector: 'app-single-post',
  templateUrl: './single-post.component.html',
  styleUrls: ['./single-post.component.css']
})
export class SinglePostComponent implements OnInit {

  postVM: PostSingleVM;
  postId : number;
  isAdmin: boolean;
  constructor(private route: ActivatedRoute, private service: BlogService, private router: Router, private auth: AuthService) { }

  ngOnInit(): void {
    this.postId = Number.parseInt(this.route.snapshot.paramMap.get('id')!);
    if(this.postId) 
    {
      this.service.getPost(this.postId).subscribe(p => this.postVM = p);
    }
    this.auth.claims.subscribe((res) => this.isAdmin = res ? res.role.includes("Admins") : false);
  }

  updateComments() {
    this.service.getPost(this.postId).subscribe(p => this.postVM = p);
  }

  clickDelete() {
    this.router.navigate(['list']);
  }

  clickDeleteCommentary(id: number)
  {
    this.service.deleteCommentary(id).subscribe(p => {
      this.updateComments();
    });
  }

}
