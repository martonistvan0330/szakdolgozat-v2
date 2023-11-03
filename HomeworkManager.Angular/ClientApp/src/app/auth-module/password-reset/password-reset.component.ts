import { Component, inject, OnInit } from '@angular/core';
import { Errors, NewUser, PasswordResetRequest } from "../../shared-module";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import {
  containsDigitValidator,
  containsLowerCaseValidator,
  containsUpperCaseValidator,
  equalValueValidator, NavigationItems, SnackBarService
} from "../../core-module";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthService } from "../../services";

@Component({
  selector: 'hwm-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.scss']
})
export class PasswordResetComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private authService = inject(AuthService);
  private snackBarService = inject(SnackBarService);
  private token = '';
  protected readonly Errors = Errors;
  passwordResetForm!: FormGroup;
  isLoading = false;
  passwordError = Errors.NoError;
  confirmPasswordError = Errors.NoError;

  get password() {
    return this.passwordResetForm.get('password')!!;
  }

  get confirmPassword() {
    return this.passwordResetForm.get('confirmPassword')!!;
  }

  ngOnInit() {
    this.formSetup();

    this.passwordControlSetup();
    this.confirmPasswordControlSetup();

    this.route.queryParamMap
      .subscribe(params => {
        this.token = params.get('token') ?? '';
      });
  }

  resetPassword() {
    if (this.passwordResetForm.invalid) {
      return;
    }

    this.isLoading = true;

    const passwordResetRequest = new PasswordResetRequest();
    passwordResetRequest.password = this.password.value;
    passwordResetRequest.token = this.token;

    this.authService.resetPassword(passwordResetRequest)
      .subscribe({
        next: () => {
          this.router.navigate([NavigationItems.login.navigationUrl])
            .then(success => {});
        },
        error: error => {
          this.snackBarService.error('Password reset failed', error.error);
          this.isLoading = false;
        }
      });
  }

  private formSetup() {
    this.passwordResetForm = new FormGroup({
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        containsLowerCaseValidator(1),
        containsUpperCaseValidator(1),
        containsDigitValidator(1)
      ]),
      confirmPassword: new FormControl('', [
        Validators.required,
        equalValueValidator('password')
      ])
    });
  }

  private passwordControlSetup() {
    this.password.statusChanges
      .subscribe(() => {
        if (this.password.status === 'VALID') {
          this.passwordError = Errors.NoError;
        }

        if (this.password.status === 'INVALID') {
          if (this.password.errors?.['required']) {
            this.passwordError = Errors.Required;
          } else if (this.password.errors?.['minlength']) {
            this.passwordError = Errors.MinLength;
          } else if (this.password.errors?.['containsLowerCase']) {
            this.passwordError = Errors.ContainsLowerCase;
          } else if (this.password.errors?.['containsUpperCase']) {
            this.passwordError = Errors.ContainsUpperCase;
          } else if (this.password.errors?.['containsDigit']) {
            this.passwordError = Errors.ContainsDigit;
          }
        }
      });
  }

  private confirmPasswordControlSetup() {
    this.confirmPassword.statusChanges
      .subscribe(() => {
        if (this.confirmPassword.status === 'VALID') {
          this.confirmPasswordError = Errors.NoError;
        }

        if (this.confirmPassword.status === 'INVALID') {
          if (this.confirmPassword.errors?.['required']) {
            this.confirmPasswordError = Errors.Required;
          } else if (this.confirmPassword.errors?.['notEqual']) {
            this.confirmPasswordError = Errors.Equal;
          }
        }
      });
  }
}
