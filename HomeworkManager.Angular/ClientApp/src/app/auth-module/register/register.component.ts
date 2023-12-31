import { Component, inject, OnInit } from '@angular/core';
import {
  containsDigitValidator,
  containsLowerCaseValidator,
  containsUpperCaseValidator,
  NavigationItems,
  SnackBarService
} from "../../core-module";
import { Router } from "@angular/router";
import { AuthService } from "../../services";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Errors, NewUser } from "../../shared-module";
import { UniqueEmailAsyncValidator } from "./validation/unique-email/unique-email-async-validator.directive";
import { UniqueUsernameAsyncValidator } from "./validation/unique-username/unique-username-async-validator.directive";

@Component({
  selector: 'hwm-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBarService = inject(SnackBarService);
  private uniqueEmailAsyncValidator = inject(UniqueEmailAsyncValidator);
  private uniqueUsernameAsyncValidator = inject(UniqueUsernameAsyncValidator);
  protected readonly Errors = Errors;
  registerForm!: FormGroup;

  isLoading = false;

  emailError = Errors.NoError;
  usernameError = Errors.NoError;
  passwordError = Errors.NoError;

  get email() {
    return this.registerForm.get('email')!!;
  }

  get username() {
    return this.registerForm.get('username')!!;
  }

  get password() {
    return this.registerForm.get('password')!!;
  }

  ngOnInit() {
    this.registerForm = new FormGroup({
      email: new FormControl('', {
        validators: [Validators.required, Validators.email],
        asyncValidators: [this.uniqueEmailAsyncValidator.validate.bind(this.uniqueEmailAsyncValidator)],
        updateOn: 'blur'
      }),
      username: new FormControl('', {
        validators: [Validators.required],
        asyncValidators: [this.uniqueUsernameAsyncValidator.validate.bind(this.uniqueUsernameAsyncValidator)],
        updateOn: 'blur'
      }),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        containsLowerCaseValidator(1),
        containsUpperCaseValidator(1),
        containsDigitValidator(1)
      ])
    });

    this.email.valueChanges
      .subscribe(() => {
        if (this.email.valid) {
          this.emailError = Errors.NoError;
        } else if (this.email.errors?.['required']) {
          this.emailError = Errors.Required;
        } else if (this.email.errors?.['email']) {
          this.emailError = Errors.Email;
        } else if (this.email.errors?.['uniqueEmail']) {
          this.emailError = Errors.Unique;
        }
      });

    this.username.valueChanges
      .subscribe(() => {
        if (this.username.valid) {
          this.usernameError = Errors.NoError;
        } else if (this.username.errors?.['required']) {
          this.usernameError = Errors.Required;
        } else if (this.username.errors?.['uniqueUsername']) {
          this.usernameError = Errors.Unique;
        }
      });

    this.password.valueChanges
      .subscribe(() => {
        if (this.password.valid) {
          this.passwordError = Errors.NoError;
        } else if (this.password.errors?.['required']) {
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
      });
  }

  register() {
    if (this.registerForm.invalid) {
      return;
    }

    this.isLoading = true;

    const newUser = new NewUser();
    newUser.email = this.email.value;
    newUser.username = this.username.value;
    newUser.password = this.password.value;

    this.authService.register(newUser)
      .subscribe({
        next: () => {
          this.router.navigate([NavigationItems.home.navigationUrl])
            .then(success => {
              if (success) {
                this.snackBarService.success('Successful registration', '');
              }
            });
        },
        error: error => {
          this.snackBarService.error('Registration failed', error.error);
          this.isLoading = false;
        }
      });
  }
}
