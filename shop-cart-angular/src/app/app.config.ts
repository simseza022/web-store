// src/app/app.config.ts

import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {providePrimeNG} from 'primeng/config';
import {definePreset} from '@primeng/themes';
import Lara from '@primeng/themes/lara';
import {provideHttpClient} from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideAnimations(),
    provideAnimationsAsync(),
    provideHttpClient(),
    providePrimeNG({
      theme: {
        options: {
          darkModeSelector: '.my-app-dark'
        },
        preset: definePreset(Lara, {
          semantic: {
            primary: {
              50: '{stone.50}',
              100: '{stone.100}',
              200: '{stone.200}',
              300: '{stone.300}',
              400: '{stone.400}',
              500: '{stone.500}',
              600: '{stone.600}',
              700: '{stone.700}',
              800: '{stone.800}',
              900: '{stone.900}',
              950: '{stone.950}'
            },
            colorScheme: {
              light: {
                primary: {
                  color: '{zinc.950}',
                  inverseColor: '#ffffff',
                  hoverColor: '{zinc.900}',
                  activeColor: '{zinc.800}'
                },
                highlight: {
                  background: '{zinc.950}',
                  focusBackground: '{zinc.700}',
                  color: '#ffffff',
                  focusColor: '#ffffff'
                }
              },
              dark: {
                primary: {
                  color: '{zinc.50}',
                  inverseColor: '{zinc.950}',
                  hoverColor: '{zinc.100}',
                  activeColor: '{zinc.200}'
                },
                highlight: {
                  background: 'rgba(250, 250, 250, .16)',
                  focusBackground: 'rgba(250, 250, 250, .24)',
                  color: 'rgba(255,255,255,.87)',
                  focusColor: 'rgba(255,255,255,.87)'
                }
              }
            }
          }
        }),
      }
    })
  ]
};
