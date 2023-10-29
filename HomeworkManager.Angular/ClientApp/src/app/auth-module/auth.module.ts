import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { MatButtonModule } from "@angular/material/button";
import { MatListModule } from "@angular/material/list";
import { MatSidenavModule } from "@angular/material/sidenav";
import { RouterLink } from "@angular/router";
import { MatCardModule } from "@angular/material/card";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AuthLayoutComponent } from './auth-layout/auth-layout.component';
import { CoreModule } from "../core-module/core.module";
import { UniqueEmailAsyncValidatorDirective } from './register/validation/unique-email/unique-email-async-validator.directive';
import { UniqueUsernameAsyncValidatorDirective } from './register/validation/unique-username/unique-username-async-validator.directive';


@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent,
    AuthLayoutComponent,
    UniqueEmailAsyncValidatorDirective,
    UniqueUsernameAsyncValidatorDirective
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatListModule,
    MatSidenavModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    CoreModule
  ],
  exports: [
    LoginComponent,
    RegisterComponent,
    AuthLayoutComponent
  ]
})
export class AuthModule {
}
