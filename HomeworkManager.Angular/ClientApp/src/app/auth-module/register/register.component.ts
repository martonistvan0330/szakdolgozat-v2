import { Component, inject } from '@angular/core';
import { AuthService } from "../../core-module";
import { Router } from "@angular/router";
import { NewUser } from "../../shared-module";

@Component({
  selector: 'hwm-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  user: NewUser = new NewUser();
  private authService = inject(AuthService);
  private router = inject(Router);

  register() {
    this.authService.register(this.user)
      .subscribe(() => {
          this.router.navigateByUrl('/home')
        }
      );
  }
}
