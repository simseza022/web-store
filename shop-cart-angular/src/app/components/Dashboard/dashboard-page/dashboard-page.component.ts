import { Component } from '@angular/core';
import {Menubar} from 'primeng/menubar';
import {Router} from '@angular/router';

@Component({
  selector: 'app-dashboard-page',
  imports: [
    Menubar
  ],
  templateUrl: './dashboard-page.component.html',
  styleUrl: './dashboard-page.component.css'
})
export class DashboardPageComponent {
  title = 'shop-cart-angular';
  items = [
    {
      label: 'Router',
      icon: 'pi pi-palette',
      items: [
        {
          label: 'Installation',
          routerLink: '/installation'
        },
        {
          label: 'Configuration',
          routerLink: '/configuration'
        }
      ]
    },
    {
      label: 'Programmatic',
      icon: 'pi pi-link',
      command: () => {
        this.router.navigate(['/installation']);
      }
    },
    {
      label: 'External',
      icon: 'pi pi-home',
      items: [
        {
          label: 'Angular',
          url: 'https://angular.io/'
        },
        {
          label: 'Vite.js',
          url: 'https://vitejs.dev/'
        }
      ]
    }
  ];
  products: any[] = [1,2,3,4,5,5,6,7];

  responsiveOptions: any[] | undefined;

  constructor(private router: Router) {
  }
  ngOnInit(): void {
    this.responsiveOptions = [
      {
        breakpoint: '1400px',
        numVisible: 2,
        numScroll: 1
      },
      {
        breakpoint: '1199px',
        numVisible: 3,
        numScroll: 1
      },
      {
        breakpoint: '767px',
        numVisible: 2,
        numScroll: 1
      },
      {
        breakpoint: '575px',
        numVisible: 1,
        numScroll: 1
      }
    ]
  }
  toggleDarkMode() {
    const element = document.querySelector('html');
    if (element != null){
      element.classList.toggle('my-app-dark');
    }
  }
  getSeverity(status: string) {
    switch (status) {
      case 'INSTOCK':
        return 'success';
      case 'LOWSTOCK':
        return 'warn';
      default ://OUTOFSTOCK
        return 'danger';
    }
  }

}
