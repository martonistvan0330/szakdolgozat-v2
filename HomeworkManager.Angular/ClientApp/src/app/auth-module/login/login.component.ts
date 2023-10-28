import { Component, inject } from '@angular/core';
import { NavigationItems, SnackBarService } from "../../core-module";
import { Router } from "@angular/router";
import { AuthenticationRequest } from "../../shared-module";
import { AuthService } from "../../services";

@Component({
  selector: 'hwm-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBarService = inject(SnackBarService);
  authRequest: AuthenticationRequest = new AuthenticationRequest();

  login() {
    this.authService.login(this.authRequest)
      .subscribe({
        next: () => {
          this.router.navigate([NavigationItems.home.navigationUrl]).then(_ => {
          });
        },
        error: error => {
          this.snackBarService.error('Login failed', error.error);
        }
      });
  }
}
