import {Component, OnInit} from '@angular/core';
import {Router, RouterOutlet} from '@angular/router';
import {Button} from 'primeng/button';
import {Chip} from 'primeng/chip';
import {Menubar} from 'primeng/menubar';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: true,
  imports: [
    RouterOutlet
  ],
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  constructor(private router: Router) {

  }

  ngOnInit(): void {
  }



}
