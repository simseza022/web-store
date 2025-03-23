import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRoutingModule } from './login-routing.module';
import { LoginPageComponent } from './login-page/login-page.component';
import { SignupPageComponent } from './signup-page/signup-page.component';
import { FogortPasswordPageComponent } from './fogort-password-page/fogort-password-page.component';
import {ReactiveFormsModule} from "@angular/forms";
import {InputText} from 'primeng/inputtext';
import {PasswordDirective} from 'primeng/password';


@NgModule({
  declarations: [
    LoginPageComponent,
    SignupPageComponent,
    FogortPasswordPageComponent
  ],
  imports: [
    CommonModule,
    LoginRoutingModule,
    ReactiveFormsModule,
    InputText,
    PasswordDirective
  ],
  exports:[
    ReactiveFormsModule,
    InputText,
    PasswordDirective
  ]
})
export class LoginModule { }
