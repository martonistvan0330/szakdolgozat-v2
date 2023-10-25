import { Component, inject } from '@angular/core';
import { AuthService, NavigationItems } from "../../core-module";
import { Router } from "@angular/router";
import { AuthenticationRequest } from "../../shared-module";

@Component({
  selector: 'hwm-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  authRequest: AuthenticationRequest = new AuthenticationRequest();

  login() {
    this.authService.login(this.authRequest)
      .subscribe(
        () => {
          this.router.navigate([NavigationItems.home.navigationUrl]).then(_ => {
          });
        }
      );
  }
}
