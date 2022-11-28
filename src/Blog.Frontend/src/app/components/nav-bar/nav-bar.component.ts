import { Component, OnInit } from '@angular/core';
import { faBook } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from 'src/app/services/auth.service';
@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  
  isLoggedIn: boolean;

  faBook = faBook;

  constructor(private auth: AuthService) { }

  ngOnInit(): void {
    this.auth.isLoggedIn.subscribe((res) => this.isLoggedIn = res)
  }

}
