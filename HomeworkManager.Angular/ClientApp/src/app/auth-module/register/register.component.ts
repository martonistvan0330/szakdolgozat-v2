import { Component, inject, OnInit } from '@angular/core';
import {
  containsDigitValidator,
  containsLowerCaseValidator,
  containsUpperCaseValidator,
  equalValueValidator,
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
  protected readonly NavigationItems = NavigationItems;
  registerForm!: FormGroup;
  isLoading = false;
  emailError = Errors.NoError;
  usernameError = Errors.NoError;
  passwordError = Errors.NoError;
  confirmPasswordError = Errors.NoError;

  get email() {
    return this.registerForm.get('email')!!;
  }

  get username() {
    return this.registerForm.get('username')!!;
  }

  get password() {
    return this.registerForm.get('password')!!;
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword')!!;
  }

  ngOnInit() {
    this.formSetup();

    this.emailControlSetup();
    this.usernameControlSetup();
    this.passwordControlSetup();
    this.confirmPasswordControlSetup();
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
                this.snackBarService.success('Successful registration', 'Please confirm your email');
              }
            });
        },
        error: error => {
          this.snackBarService.error('Registration failed', error.error);
          this.isLoading = false;
        }
      });
  }

  private formSetup() {
    this.registerForm = new FormGroup({
      email: new FormControl('', {
        validators: [Validators.required, Validators.email],
        asyncValidators: [this.uniqueEmailAsyncValidator.validate.bind(this.uniqueEmailAsyncValidator)],
        updateOn: 'change'
      }),
      username: new FormControl('', {
        validators: [Validators.required],
        asyncValidators: [this.uniqueUsernameAsyncValidator.validate.bind(this.uniqueUsernameAsyncValidator)],
        updateOn: 'change'
      }),
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

  private emailControlSetup() {
    this.email.statusChanges
      .subscribe(() => {
        if (this.email.status === 'VALID') {
          this.emailError = Errors.NoError;
        }

        if (this.email.status === 'INVALID') {
          if (this.email.errors?.['required']) {
            this.emailError = Errors.Required;
          } else if (this.email.errors?.['email']) {
            this.emailError = Errors.Email;
          } else if (this.email.errors?.['uniqueEmail']) {
            this.emailError = Errors.Unique;
          }
        }
      });
  }

  private usernameControlSetup() {
    this.username.statusChanges
      .subscribe(() => {
        if (this.username.status === 'VALID') {
          this.usernameError = Errors.NoError;
        }

        if (this.username.status === 'INVALID') {
          if (this.username.errors?.['required']) {
            this.usernameError = Errors.Required;
          } else if (this.username.errors?.['uniqueUsername']) {
            this.usernameError = Errors.Unique;
          }
        }
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
