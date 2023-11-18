import { Directive, forwardRef, inject, Input } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, NG_VALIDATORS, ValidationErrors } from "@angular/forms";
import { catchError, Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { AssignmentService } from "../../services/assignment.service";

export function uniqueAssignmentNameAsyncValidator(assignmentService: AssignmentService, courseId: number, groupName: string, oldName: string | null = null): AsyncValidatorFn {
  return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
    const name = control.value as string;

    if (name === oldName) {
      return of(null)
    }

    return assignmentService.nameAvailable(name, courseId, groupName)
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
  selector: 'input[type=text][uniqueAssignmentNameValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: forwardRef(() => UniqueAssignmentNameAsyncValidatorDirective),
    multi: true
  }]
})
export class UniqueAssignmentNameAsyncValidatorDirective {
  private assignmentService = inject(AssignmentService);

  @Input('uniqueAssignmentNameValidator') oldName: string | null = null;
  @Input('courseId') courseId!: number;
  @Input('groupName') groupName!: string;

  validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    return uniqueAssignmentNameAsyncValidator(this.assignmentService, this.courseId, this.groupName, this.oldName)(control);
  }

}
