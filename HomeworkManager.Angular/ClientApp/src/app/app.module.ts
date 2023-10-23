import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from "./shared-module/shared.module";
import { HomeworkManagerModule } from "./homework-manager-module/homework-manager.module";
import { CoreModule } from "./core-module/core.module";
import { ControlModule } from "./control-module/control.module";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { TokenInterceptor } from "./core-module/interceptors/token-interceptor";
import { AuthModule } from "./auth-module/auth.module";
import { AdminModule } from "./admin-module/admin.module";

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AuthModule,
    AdminModule,
    ControlModule,
    CoreModule,
    HomeworkManagerModule,
    SharedModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
