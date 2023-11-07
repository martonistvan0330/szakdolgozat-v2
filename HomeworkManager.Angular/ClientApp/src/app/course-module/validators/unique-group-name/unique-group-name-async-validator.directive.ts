import { Directive, forwardRef, inject, Input } from '@angular/core';
import { AbstractControl, AsyncValidator, AsyncValidatorFn, NG_VALIDATORS, ValidationErrors } from "@angular/forms";
import { catchError, Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { GroupService } from "../../services/group.service";

export function uniqueGroupNameAsyncValidator(groupService: GroupService, oldName: string | null = null): AsyncValidatorFn {
  return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
    const name = control.value as string;

    if (name === oldName) {
      return of(null)
    }

    return groupService.nameAvailable(name)
      .pipe(
        map(available => {
          if (!available) {
            return { uniqueName: { value: false } };
          }
          return null;
        }),
        catchError((error, caught) => {
          return of({ uniqueName: { value: false } });
        })
      );
  };
}

@Directive({
  selector: 'input[type=text][uniqueGroupNameValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: forwardRef(() => UniqueGroupNameAsyncValidatorDirective),
    multi: true
  }]
})
export class UniqueGroupNameAsyncValidatorDirective implements AsyncValidator {
  private groupService = inject(GroupService);

  @Input('uniqueGroupNameValidator') oldName: string | null = null

  validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    return uniqueGroupNameAsyncValidator(this.groupService, this.oldName)(control);
  }
}
