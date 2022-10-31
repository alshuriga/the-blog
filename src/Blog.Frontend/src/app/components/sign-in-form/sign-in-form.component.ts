import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { BlogService } from 'src/app/services/blog.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { ServerValidationService } from 'src/app/services/server-validation.service';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-sign-in-form',
  templateUrl: './sign-in-form.component.html',
  styleUrls: ['./sign-in-form.component.css']
})
export class SignInFormComponent implements OnInit {

  jwt: string;
  validErrors$: Observable<any> = this.validation.serverErrors$;

  form: FormGroup = new FormGroup({
    'username': new FormControl(),
    'password': new FormControl()
  });

  constructor(private service: BlogService, 
    private auth: AuthService, 
    private router: Router,
    private validation: ServerValidationService) { }

  ngOnInit(): void {
    this.validation.removeServerErrors();
  }

  onSubmit() {
    const obs = {
      next: (res: any) => {
        const token = new JwtHelperService().decodeToken(res.token)
        console.log(res);
        console.log(token);
        console.log(new Date(token.iat * 1000));
        console.log(new Date(token.exp * 1000));
      },
      error: (err: any) => {
        console.log(err.error.errors);
      }
      
    }
    this.auth.logIn(this.form.value);
    
   
  }

}
