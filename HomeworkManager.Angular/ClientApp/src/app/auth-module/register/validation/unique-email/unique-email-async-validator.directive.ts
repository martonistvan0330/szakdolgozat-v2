import { Directive, forwardRef, inject, Injectable } from '@angular/core';
import { AbstractControl, AsyncValidator, NG_VALIDATORS, ValidationErrors } from "@angular/forms";
import { ApiClientService } from "../../../../services";
import { catchError, Observable, of } from "rxjs";
import { map } from "rxjs/operators";

@Injectable({ providedIn: 'root' })
export class UniqueEmailAsyncValidator implements AsyncValidator {
  private apiClient = inject(ApiClientService);

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    const email = control.value as string;
    return this.apiClient.get<boolean>('User/EmailAvailable?email=' + email)
      .pipe(
        map(available => {
          if (!available) {
            return { uniqueEmail: { value: false } };
          }
          return null;
        }),
        catchError((error, caught) => {
          return of({ uniqueEmail: { value: false } });
        })
      );
  }
}

@Directive({
  selector: 'input[type=text][uniqueEmailAsync], input[type=email][uniqueEmailAsync]',
  providers: [{ provide: NG_VALIDATORS, useExisting: forwardRef(() => UniqueEmailAsyncValidatorDirective), multi: true }]
})
export class UniqueEmailAsyncValidatorDirective implements AsyncValidator {
  private validator = inject(UniqueEmailAsyncValidator);

  validate(control: AbstractControl): Observable<ValidationErrors | null> {
    return this.validator.validate(control);
  }
}
