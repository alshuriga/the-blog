import { Component, OnInit } from '@angular/core';
import { UserClaims } from 'src/app/models/UserModels';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['./user-panel.component.css']
})
export class UserPanelComponent implements OnInit {
  claims?: UserClaims;
  isLoggedIn: boolean;
  constructor(private auth: AuthService) { }

  ngOnInit(): void {
   this.auth.isLoggedIn.subscribe((res) => this.isLoggedIn = res);
   this.auth.claims.subscribe(res => this.claims = res);
  }

  onLogOutClick() {
    this.auth.logOut();
  }


}
