import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';

import { TruckRepository } from './modules/trucks/domain/repositories/truck.repository.interface';
import { ApiService } from './core/services/api/api.service';
import { AuthInterceptor } from './core/services/auth/auth.interceptor';
import { TruckApiService } from './modules/trucks/infrastructure/repositories/truck_api.service';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AuthService } from './core/services/auth/auth.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),

    ApiService,
    AuthService,
  
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },

    { provide: TruckRepository, useClass: TruckApiService }
  ]
};
