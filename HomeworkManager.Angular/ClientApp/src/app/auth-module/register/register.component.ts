import { Component, inject } from '@angular/core';
import { AuthService, NavigationItems } from "../../core-module";
import { Router } from "@angular/router";
import { NewUser } from "../../shared-module";

@Component({
  selector: 'hwm-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  user: NewUser = new NewUser();

  register() {
    this.authService.register(this.user)
      .subscribe(() => {
          this.router.navigate([NavigationItems.home.navigationUrl]).then(_ => {
          });
        }
      );
  }
}
