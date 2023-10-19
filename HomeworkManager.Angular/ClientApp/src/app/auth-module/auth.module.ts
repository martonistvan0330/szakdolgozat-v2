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
import { FormsModule } from "@angular/forms";


@NgModule({
  declarations: [
    LoginComponent,
    RegisterComponent
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
    FormsModule
  ],
  exports: [
    LoginComponent,
    RegisterComponent
  ]
})
export class AuthModule {
}
