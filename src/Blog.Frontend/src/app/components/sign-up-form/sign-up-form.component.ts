import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { User, UserSignUpDTO } from 'src/app/models/UserModels';
import { AuthService } from 'src/app/services/auth.service';
import { BlogService } from 'src/app/services/blog.service';
import { ServerValidationService } from 'src/app/services/server-validation.service';
import { url } from 'src/app/webapi-constants';

@Component({
  selector: 'app-sign-up-form',
  templateUrl: './sign-up-form.component.html',
  styleUrls: ['./sign-up-form.component.css']
})
export class SignUpFormComponent implements OnInit {


  constructor(private validation: ServerValidationService, private blog: BlogService, private auth: AuthService) { }

  validErrors$: Observable<any> = this.validation.serverErrors$;

  signupForm = new FormGroup({
    'username': new FormControl<string>('', {nonNullable: true}),
    'password': new FormControl<string>('', {nonNullable: true}),
    'email': new FormControl<string>('', {nonNullable: true}),
    'repeatPassword': new FormControl<string>('', {nonNullable: true})
  })

  ngOnInit(): void {
  }

  onSubmit() {
    const user: UserSignUpDTO  = this.signupForm.getRawValue();
    this.blog.signUp(user).subscribe(() => {
      this.auth.logIn({username: user.username, password: user.password});
    })
  }

}
