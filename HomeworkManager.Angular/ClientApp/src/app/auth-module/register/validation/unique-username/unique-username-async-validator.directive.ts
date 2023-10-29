import { Directive, forwardRef, inject, Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, NG_VALIDATORS, ValidationErrors } from "@angular/forms";
import { ApiClientService } from "../../../../services";
import { catchError, Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { UniqueEmailAsyncValidator } from "../unique-email/unique-email-async-validator.directive";

@Injectable({ providedIn: 'root' })
export class UniqueUsernameAsyncValidator implements AsyncValidator {
  private apiClient = inject(ApiClientService);

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    const username = control.value as string;
    return this.apiClient.get<boolean>('User/UsernameAvailable?username=' + username)
      .pipe(
        map(available => {
          if (!available) {
            return { uniqueUsername: { value: false } };
          }
          return null;
        }),
        catchError((error, caught) => {
          return of({ uniqueUsername: { value: false } });
        })
      );
  }
}

@Directive({
  selector: 'input[type=text][uniqueUsernameAsync], input[type=email][uniqueUsernameAsync]',
  providers: [{ provide: NG_VALIDATORS, useExisting: forwardRef(() => UniqueUsernameAsyncValidatorDirective), multi: true }]
})
export class UniqueUsernameAsyncValidatorDirective implements AsyncValidator {
  private validator = inject(UniqueEmailAsyncValidator);

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    return this.validator.validate(control);
  }
}
