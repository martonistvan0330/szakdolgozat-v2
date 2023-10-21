import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from "./environments/environment";
import { enableProdMode } from "@angular/core";

export function getApiUrl(){
  return environment.apiUrl;
}

const providers = [
  { provide: 'API_URL', useFactory: getApiUrl(), deps: [] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
