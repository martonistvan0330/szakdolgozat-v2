import { Directive, forwardRef, inject, Input } from '@angular/core';
import { AbstractControl, AsyncValidator, AsyncValidatorFn, NG_VALIDATORS, ValidationErrors } from "@angular/forms";
import { catchError, Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { CourseService } from "../../services/course.service";

export function uniqueCourseNameAsyncValidator(courseService: CourseService, oldName: string | null = null): AsyncValidatorFn {
  return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
    const name = control.value as string;

    if (name === oldName) {
      return of(null)
    }

    return courseService.nameAvailable(name)
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
  selector: 'input[type=text][uniqueCourseNameValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: forwardRef(() => UniqueCourseNameAsyncValidatorDirective),
    multi: true
  }]
})
export class UniqueCourseNameAsyncValidatorDirective implements AsyncValidator {
  private courseService = inject(CourseService);

  @Input('uniqueCourseNameValidator') oldName: string | null = null

  validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    return uniqueCourseNameAsyncValidator(this.courseService, this.oldName)(control);
  }
}
