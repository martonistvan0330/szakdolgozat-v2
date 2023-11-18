import { Component, inject, OnInit } from '@angular/core';
import { CourseModel, Errors } from "../../shared-module";
import { ActivatedRoute, Router } from "@angular/router";
import { CourseService } from "../services/course.service";
import { NavigationItems, SnackBarService } from "../../core-module";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { UpdateCourse } from "../../shared-module/models/course/update-course";
import {
  uniqueCourseNameAsyncValidator
} from "../validators/unique-course-name/unique-course-name-async-validator.directive";

@Component({
  selector: 'hwm-course-edit',
  templateUrl: './course-edit.component.html',
  styleUrls: ['./course-edit.component.scss']
})
export class CourseEditComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private courseService = inject(CourseService);
  private router = inject(Router);
  private snackBarService = inject(SnackBarService);
  protected readonly Errors = Errors;
  course: CourseModel | null = null;
  courseEditForm!: FormGroup;
  isLoading = false;
  nameError = Errors.NoError;

  get name() {
    return this.courseEditForm.get('name')!!;
  }

  get description() {
    return this.courseEditForm.get('description')!!;
  }

  ngOnInit() {
    this.activatedRoute.data
      .subscribe(({ course }) => {
        const courseModel = course as CourseModel;
        this.course = courseModel;

        if (courseModel) {
          this.setup()
        } else {
          this.snackBarService.error('Something went wrong!');
        }
      });
  }

  update() {
    if (this.courseEditForm.invalid) {
      return;
    }

    this.isLoading = true;

    const updatedCourse = new UpdateCourse();
    updatedCourse.name = this.name.value;
    updatedCourse.description = this.description.value ? this.description.value : null;

    this.courseService.updateCourse(this.course!!.courseId, updatedCourse)
      .subscribe({
        next: success => {
          this.router.navigate([NavigationItems.courseDetails.navigationUrl, this.course!!.courseId])
            .then(success => {
              if (success) {
                this.snackBarService.success('Course updated');
              }
            });
        },
        error: error => {
          this.snackBarService.error('Course update failed', error.error);
          this.isLoading = false;
        }
      });
  }

  private setup() {
    this.formSetup();

    this.nameControlSetup();
  }

  private formSetup() {
    this.courseEditForm = new FormGroup({
      name: new FormControl(this.course!!.name, {
        validators: [Validators.required],
        asyncValidators: [uniqueCourseNameAsyncValidator(this.courseService, this.course?.name)],
        updateOn: 'change'
      }),
      description: new FormControl(this.course!!.description),
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
