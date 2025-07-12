import {UserLogin} from '../models/user-login.model';
import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {UserLoginResponse} from '../models/user-login-response.model';
import moment from 'moment';
import {catchError, map} from 'rxjs';

@Injectable({providedIn: 'root'})
export class AuthService {
  expiresIn = 'expires_in';
  idToken = 'id_token';

  constructor(private http: HttpClient) {}

  get getExpiration(){
    const expiration = localStorage.getItem(this.expiresIn);
    const expiresAt = expiration != null ? JSON.parse(expiration) : null;
    return moment(expiresAt);
  }

  login(model:UserLogin){
    return this.http.post<UserLoginResponse>("https://localhost:7271/api/auth/login",model)
      .pipe(
        map((response:UserLoginResponse)=> this.setSession(response)),
      );
  }
  private setSession(response:UserLoginResponse) {
    const expiresIn = moment().add(response.expires_in,'second');

    localStorage.setItem(this.idToken, response.token);
    localStorage.setItem(this.expiresIn, JSON.stringify(expiresIn.valueOf()) );
  }

  logout(){
    localStorage.removeItem(this.idToken);
    localStorage.removeItem(this.expiresIn);
  }

  isUserLoggedOut(){
    return !this.isUserLoggedIn();
  }

  isUserLoggedIn(){
    return moment().isBefore(this.getExpiration);
  }
}
