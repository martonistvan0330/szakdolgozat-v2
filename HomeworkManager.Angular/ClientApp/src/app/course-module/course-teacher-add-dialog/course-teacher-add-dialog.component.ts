import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { SnackBarService } from "../../core-module";
import { ColumnDefinition, UserListRow } from "../../shared-module";
import { BehaviorSubject, catchError, Observable, of, startWith } from "rxjs";
import { FormControl, FormGroup } from "@angular/forms";
import { MatAutocompleteSelectedEvent } from "@angular/material/autocomplete";
import { map } from "rxjs/operators";
import { CourseService } from "../services/course.service";

@Component({
  selector: 'hwm-course-teacher-add-dialog',
  templateUrl: './course-teacher-add-dialog.component.html',
  styleUrls: ['./course-teacher-add-dialog.component.scss']
})
export class CourseTeacherAddDialogComponent {
  private dialogRef = inject(MatDialogRef<CourseTeacherAddDialogComponent>);
  private courseService = inject(CourseService);
  private snackBarService = inject(SnackBarService);
  private courseId: number = inject(MAT_DIALOG_DATA)
  private teachers: UserListRow[] = [];
  private selectedTeachers: UserListRow[] = [];
  private selectedTeachersSubject = new BehaviorSubject(this.selectedTeachers);
  filteredTeachers!: Observable<UserListRow[]>;
  selectedTeachers$ = this.selectedTeachersSubject.asObservable();
  teacherForm!: FormGroup;
  columnDefs = [
    new ColumnDefinition('Full name', 'fullName'),
    new ColumnDefinition('Email', 'email')
  ];

  get searchText() {
    return this.teacherForm.get('searchText')!!;
  }

  ngOnInit() {
    this.courseService.getAddableTeachers(this.courseId)
      .subscribe({
        next: teachers => {
          this.teachers = teachers;
        },
        error: error => {
          this.snackBarService.error('Something went wrong', error.error);
          this.dialogRef.close();
        }
      });

    this.setupForm();
  }

  onOptionSelected(event: MatAutocompleteSelectedEvent) {
    this.selectedTeachers.push(event.option.value as UserListRow);
    this.selectedTeachers
      .sort((a, b) => {
        if (a.fullName < b.fullName) {
          return -1;
        } else if (a.fullName == b.fullName) {
          return 0;
        } else {
          return 1
        }
      })

    this.updateTable()

    this.searchText.reset('');
  }

  onRemoveClick(teacher: UserListRow) {
    const index = this.selectedTeachers.indexOf(teacher);
    this.selectedTeachers.splice(index, 1);

    this.updateTable()
  }

  onOkClick() {
    const selectedTeacherIds = this.selectedTeachers
      .map(teacher => teacher.userId);

    this.courseService.addTeachers(this.courseId, selectedTeacherIds)
      .pipe(
        catchError(() => {
          this.snackBarService.error('Something went wrong');
          return of();
        })
      )
      .subscribe(() => {
        this.dialogRef.close();
      });
  }

  onCancelClick() {
    this.dialogRef.close();
  }

  displayFn(teacher: UserListRow): string {
    return teacher
      ? teacher.fullName + '(' + teacher.email + ')'
      : '';
  }

  private setupForm() {
    this.teacherForm = new FormGroup({
      searchText: new FormControl('')
    });

    this.filteredTeachers = this.searchText.valueChanges
      .pipe(
        startWith(''),
        map(value => this.filter(value || ''))
      );
  }

  private filter(value: string) {
    const filterValue = value.toString().toLowerCase();


    return this.teachers.filter(teacher => {


      return !this.selectedTeachers.includes(teacher)
        && (teacher.fullName.toLowerCase().includes(filterValue)
          || teacher.email.toLowerCase().includes(filterValue));
    })
  }

  private updateTable() {
    this.selectedTeachersSubject.next(this.selectedTeachers);
  }
}
