import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Errors, NewCourse } from "../../shared-module";
import {
  uniqueCourseNameAsyncValidator
} from "../validators/unique-course-name/unique-course-name-async-validator.directive";
import { CourseService } from "../services/course.service";
import { Router } from "@angular/router";
import { NavigationItems, SnackBarService } from "../../core-module";

@Component({
  selector: 'hwm-course-create',
  templateUrl: './course-create.component.html',
  styleUrls: ['./course-create.component.scss']
})
export class CourseCreateComponent implements OnInit {
  private courseService = inject(CourseService);
  private router = inject(Router);
  private snackBarService = inject(SnackBarService);
  protected readonly Errors = Errors;
  courseCreateForm!: FormGroup;
  isLoading = false;
  nameError = Errors.NoError;

  get name() {
    return this.courseCreateForm.get('name')!!;
  }

  get description() {
    return this.courseCreateForm.get('description')!!;
  }

  ngOnInit() {
    this.formSetup();

    this.nameControlSetup();
  }

  create() {
    if (this.courseCreateForm.invalid) {
      return;
    }

    this.isLoading = true;

    const newCourse = new NewCourse();
    newCourse.name = this.name.value;
    newCourse.description = this.description.value ? this.description.value : null;

    this.courseService.createCourse(newCourse)
      .subscribe({
        next: courseId => {
          this.isLoading = false;
          this.router.navigate([NavigationItems.courseDetails.navigationUrl, courseId])
            .then(success => {
              if (success) {
                this.snackBarService.success('Course created');
              }
            });
        },
        error: error => {
          this.snackBarService.error('Course creation failed', error.error);
          this.isLoading = false;
        }
      });
  }

  private formSetup() {
    this.courseCreateForm = new FormGroup({
      name: new FormControl('', {
        validators: [Validators.required,],
        asyncValidators: [uniqueCourseNameAsyncValidator(this.courseService)],
        updateOn: 'change'
      }),
      description: new FormControl(''),
    });
  }

  private nameControlSetup() {
    this.name.statusChanges
      .subscribe(() => {
        if (this.name.status === 'VALID') {
          this.nameError = Errors.NoError;
        }

        if (this.name.status === 'INVALID') {
          if (this.name.errors?.['required']) {
            this.nameError = Errors.Required;
          } else if (this.name.errors?.['uniqueName']) {
            this.nameError = Errors.Unique;
          }
        }
      });
  }
}
