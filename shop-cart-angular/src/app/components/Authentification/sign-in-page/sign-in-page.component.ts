import {Component} from '@angular/core';
import {Button} from 'primeng/button';
import {Chip} from 'primeng/chip';
import {AuthService} from '../../../services/auth-service';
import {Router} from '@angular/router';
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {LOAD_STATE} from '../../../utilities/load-state';
import {Location, NgIf} from '@angular/common';

@Component({
  selector: 'app-sign-in-page',
  imports: [
    Button,
    Chip,
    ReactiveFormsModule,
    NgIf
  ],
  templateUrl: './sign-in-page.component.html',
  styleUrl: './sign-in-page.component.css'
})
export class SignInPageComponent {
  loadState:LOAD_STATE = LOAD_STATE.NOT_LOADING;
  signInForm: FormGroup;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder,
  ) {
    this.signInForm = fb.group({
      Email: new FormControl('', Validators.required),
      Password: new FormControl('', Validators.required),
    });
  }
  hasErrors(control:string){
    return this.signInForm.get(control)?.dirty
      && this.signInForm.get(control)?.touched
      && this.signInForm.get(control)?.hasError('required')
  }

  login() {
    console.log(this.signInForm.value);
    if (this.signInForm.valid) {
      this.loadState = LOAD_STATE.LOADING;
      this.authService.login(this.signInForm.value).subscribe({
        next: result => {
          this.router.navigateByUrl('/', { replaceUrl: true });
          console.log("Successfully logged in");
          this.loadState = LOAD_STATE.LOADED;
        },
        error: error => {
          console.log("Error logging in");
          this.loadState = LOAD_STATE.ERROR;
        }
      })
    }

  }

  protected readonly LOAD_STATE = LOAD_STATE;
}
