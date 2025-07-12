import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import {SignInPageComponent} from './components/Authentification/sign-in-page/sign-in-page.component';
import {DashboardPageComponent} from './components/Dashboard/dashboard-page/dashboard-page.component'; // Or your other components

export const routes: Routes = [
  { path: '', component: DashboardPageComponent },
  { path: 'sign-in', component: SignInPageComponent }
];
