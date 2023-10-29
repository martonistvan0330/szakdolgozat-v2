import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator, ValidatorFn } from "@angular/forms";

export function containsDigitValidator(minCount: number = 1): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (minCount <= 0) {
      return null;
    }

    let count = 0;

    const value = control.value as string;
    for (const char of value) {
      if ('0' <= char && char <= '9') {
        count++;
      }
    }
    
    if (count < minCount) {
      return { containsDigit: { value: control.value } };
    }

    return null;
  };
}

@Directive({
  selector: 'input[type=text][containsDigit], input[type=email][containsDigit], input[type=password][containsDigit]',
  providers: [{ provide: NG_VALIDATORS, useExisting: ContainsDigitValidatorDirective, multi: true }]
})
export class ContainsDigitValidatorDirective implements Validator {
  @Input('containsDigit') minCount = 1;

  validate(control: AbstractControl): ValidationErrors | null {
    return this.minCount > 0 ? containsDigitValidator(this.minCount)(control) : null;
  }
}
