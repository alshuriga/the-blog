import { Injectable } from '@angular/core';
import { BlogService } from './blog.service';
import { User, UserClaims } from '../models/UserModels';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private _isLoggedIn = new BehaviorSubject<boolean>(false);
  private _claims = new BehaviorSubject<UserClaims | undefined>(undefined);
  isLoggedIn = this._isLoggedIn.asObservable();
  claims = this._claims.asObservable();

  constructor(private api: BlogService, private router: Router) { 
    this.authCheck();
    console.log('auth service')
  }

  logIn(user: User) {
    this.api.signIn(user).subscribe(res => {
      if(res.token)
      {
        localStorage.setItem('jwt', res.token);
        const decoded = new JwtHelperService().decodeToken(res.token);
        if(Number.parseInt(decoded.exp) * 1000 < Date.now()) return;
        let claims: UserClaims ={
          username: decoded.sub,
          role: decoded.role
        }
        localStorage.setItem('claims', JSON.stringify(claims));
        localStorage.setItem('exp', decoded.exp); 
        localStorage.setItem('roles', decoded.roles);
        this.authCheck();
        this.router.navigate(['list']);
      }
    });
  
  }

  logOut() {
    localStorage.clear();
    this.authCheck();
  }



  authCheck() {
    const jwt = localStorage.getItem('jwt');
   
    if(jwt)
    {
      const decoded = new JwtHelperService().decodeToken(jwt);
      if(Number.parseInt(decoded.exp) * 1000 >= Date.now()) this._isLoggedIn.next(true);
      const claims = localStorage.getItem('claims');
      if(claims)
      {
        this._claims.next(JSON.parse(claims) as UserClaims);
      }
      return;
    }

    this._isLoggedIn.next(false);

  }
}
