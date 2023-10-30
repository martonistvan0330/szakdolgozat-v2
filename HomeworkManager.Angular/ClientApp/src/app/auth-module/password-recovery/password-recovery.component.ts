import { Component, inject, ViewChild } from '@angular/core';
import { NavigationItems, SnackBarService } from "../../core-module";
import { NgForm } from "@angular/forms";
import { PasswordRecoveryRequest } from "../../shared-module";
import { AuthService } from "../../services";

@Component({
  selector: 'hwm-password-recovery',
  templateUrl: './password-recovery.component.html',
  styleUrls: ['./password-recovery.component.scss']
})
export class PasswordRecoveryComponent {
  private authService = inject(AuthService);
  private snackBarService = inject(SnackBarService);
  protected readonly NavigationItems = NavigationItems;
  @ViewChild('passwordRecoveryForm') passwordRecoveryForm!: NgForm;
  passwordRecoveryRequest = new PasswordRecoveryRequest();

  recoverPassword() {
    if (this.passwordRecoveryForm.invalid) {
      return;
    }

    this.authService.recoverPassword(this.passwordRecoveryRequest)
      .subscribe(() => {
        this.snackBarService.info('Recovery email has been sent')
      });
  }
}