import {Component, inject} from '@angular/core';
import {AuthenticationRequest} from "../../shared-module/models/auth/authentication-request";
import {AuthService} from "../../core-module";
import {Router} from "@angular/router";
import {UserModel} from "../../shared-module/models/auth/user-model";

@Component({
  selector: 'hwm-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  user: UserModel = new UserModel();
  private authService = inject(AuthService);
  private router = inject(Router);

  register() {
    this.authService.register(this.user)
      .subscribe(
        () => {
          this.router.navigateByUrl('/home')
        }
      );
  }
}
