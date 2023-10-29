import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator, ValidatorFn } from "@angular/forms";

export function containsUpperCaseValidator(minCount: number = 1): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (minCount <= 0) {
      return null;
    }

    let count = 0;

    const value = control.value as string;
    for (const char of value) {
      if ('A' <= char && char <= 'Z') {
        count++;
      }
    }

    if (count < minCount) {
      return { containsUpperCase: { value: control.value } };
    }

    return null;
  };
}

@Directive({
  selector: 'input[type=text][containsUpperCase], input[type=email][containsUpperCase], input[type=password][containsUpperCase]',
  providers: [{ provide: NG_VALIDATORS, useExisting: ContainsUpperCaseValidatorDirective, multi: true }]
})
export class ContainsUpperCaseValidatorDirective implements Validator {
  @Input('containsUpperCase') minCount = 1;

  validate(control: AbstractControl): ValidationErrors | null {
    return this.minCount > 0 ? containsUpperCaseValidator(this.minCount)(control) : null;
  }
}
