import { Component } from '@angular/core';
import {FormBuilder, FormControl, FormGroup} from '@angular/forms';

@Component({
  selector: 'app-login-page',
  standalone: false,
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css'
})
export class LoginPageComponent {
  signInForm = new FormGroup({
    Email: new FormControl(''),
    password: new FormControl(''),
  });

  get Email():FormControl<string | null>{
    return this.signInForm.controls.Email;
  }
  get Password():FormControl<string | null>{
    return this.signInForm.controls.Email;
  }
  isSubmitting=false
  constructor(private fb:FormBuilder) {
  }
}
