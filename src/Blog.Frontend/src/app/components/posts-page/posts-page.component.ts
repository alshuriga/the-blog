import { Component, OnInit } from '@angular/core';
import { PostsPage } from 'src/app/models/PostModels';
import { BlogService } from 'src/app/services/blog.service';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';


@Component({
  selector: 'app-posts-page',
  templateUrl: './posts-page.component.html',
  styleUrls: ['./posts-page.component.css']
})
export class PostsPageComponent implements OnInit {

  page!: PostsPage;
  
  pageNum: number;
  isDraft: boolean;
  tagName: string | undefined;
  constructor(private blogService: BlogService, private route: ActivatedRoute) {
   }

  ngOnInit(): void {
      this.tagName = undefined;
      this.pageNum = 0;
      this.isDraft = this.route.snapshot.url[0].path === 'drafts';
      this.blogService.getPostsPage(this.pageNum, this.isDraft, this.tagName)
      .subscribe(page => this.page = page);
  
      this.route.queryParamMap.subscribe(p => {
        if(p.get('tag')) 
        this.filterByTag(p.get('tag')!);
      });
    };
  

  filterByTag(tag: string)
  {
    this.tagName = tag;
    this.blogService.getPostsPage(0, this.isDraft, this.tagName)
      .subscribe(page => this.page = page);
  }

  changePage(page: number)
  {
    this.pageNum = page;
    this.blogService.getPostsPage(this.pageNum, this.isDraft, this.tagName)
      .subscribe(page => this.page = page);
  }

  onClickDelete() 
  {
    this.blogService.getPostsPage(this.pageNum, this.isDraft, this.tagName)
      .subscribe(page => this.page = page);
  }
}
