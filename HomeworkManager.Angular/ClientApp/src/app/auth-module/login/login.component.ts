import { Component, inject } from '@angular/core';
import { AuthService } from "../../core-module";
import { Router } from "@angular/router";
import { AuthenticationRequest } from "../../shared-module/models/auth/authentication-request";

@Component({
  selector: 'hwm-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  authRequest: AuthenticationRequest = new AuthenticationRequest();
  private authService = inject(AuthService);
  private router = inject(Router);

  login() {
    this.authService.login(this.authRequest)
      .subscribe(
        () => {
          this.router.navigateByUrl('/home')
        }
      );
  }
}
