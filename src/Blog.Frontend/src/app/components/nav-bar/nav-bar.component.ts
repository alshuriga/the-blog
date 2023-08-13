import { Component, OnInit } from '@angular/core';
import { faBook } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from 'src/app/services/auth.service';
@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  
  isAdminLoggedIn: boolean;

  faBook = faBook;

  constructor(private auth: AuthService) { }

  ngOnInit(): void {
    this.auth.claims.subscribe((res) => this.isAdminLoggedIn = res ? res!.role.includes("Admin") : false);
  }

}
