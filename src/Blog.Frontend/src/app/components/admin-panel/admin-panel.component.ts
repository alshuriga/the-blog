import { Component, OnInit } from '@angular/core';
import { UserListDTO, UsersListVM } from 'src/app/models/UserModels';
import { BlogService } from 'src/app/services/blog.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  public userListVm: UsersListVM;

  constructor(private blog: BlogService) { }

  ngOnInit(): void {
    this.blog.getUsersList().subscribe(res => {
      this.userListVm = res;
    })
  }

  onClickDelete(id: string) {
    this.blog.deleteUser(id).subscribe(res => {
      this.blog.getUsersList().subscribe(res => {
        this.userListVm = res;
      });
    });
  }
  onClickToggle(id: string) {
    this.blog.toggleAdmin(id).subscribe(res => {
      this.blog.getUsersList().subscribe(res => {
        this.userListVm = res;
      });
    });
  }

  
}
