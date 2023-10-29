import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator, ValidatorFn } from "@angular/forms";

export function containsLowerCaseValidator(minCount: number = 1): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (minCount <= 0) {
      return null;
    }

    let count = 0;

    const value = control.value as string;
    for (const char of value) {
      if ('a' <= char && char <= 'z') {
        count++;
      }
    }

    if (count < minCount) {
      return { containsLowerCase: { value: control.value } };
    }

    return null;
  };
}

@Directive({
  selector: 'input[type=text][containsLowerCase], input[type=email][containsLowerCase], input[type=password][containsLowerCase]',
  providers: [{ provide: NG_VALIDATORS, useExisting: ContainsLowerCaseValidatorDirective, multi: true }]
})
export class ContainsLowerCaseValidatorDirective implements Validator {
  @Input('containsLowerCase') minCount = 1;

  validate(control: AbstractControl): ValidationErrors | null {
    return this.minCount > 0 ? containsLowerCaseValidator(this.minCount)(control) : null;
  }
}
