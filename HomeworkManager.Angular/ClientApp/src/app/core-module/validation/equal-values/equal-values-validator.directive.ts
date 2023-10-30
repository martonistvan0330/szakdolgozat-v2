import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator, ValidatorFn } from "@angular/forms";

export function equalValueValidator(otherControlName: string = ''): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.parent) {
      return null;
    }

    const otherControl = control.parent.get(otherControlName);

    if (control && otherControl && control.value === otherControl.value) {
      return null;
    }

    return { notEqual: { value: control.value, otherValue: otherControl!!.value } };
  };
}


@Directive({
  selector: '[equalValues]',
  providers: [{ provide: NG_VALIDATORS, useExisting: EqualValuesValidatorDirective, multi: true }]
})
export class EqualValuesValidatorDirective implements Validator {
  @Input('equalValues') otherControlName!: string;

  validate(control: AbstractControl): ValidationErrors | null {
    return equalValueValidator(this.otherControlName)(control);
  }

}
