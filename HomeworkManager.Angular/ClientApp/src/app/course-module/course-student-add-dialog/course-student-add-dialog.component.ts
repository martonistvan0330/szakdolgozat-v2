import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { CourseService } from "../services/course.service";
import { SnackBarService } from "../../core-module";
import { ColumnDefinition, UserListRow } from "../../shared-module";
import { BehaviorSubject, catchError, Observable, of, startWith } from "rxjs";
import { FormControl, FormGroup } from "@angular/forms";
import { MatAutocompleteSelectedEvent } from "@angular/material/autocomplete";
import { map } from "rxjs/operators";

@Component({
  selector: 'hwm-course-student-add-dialog',
  templateUrl: './course-student-add-dialog.component.html',
  styleUrls: ['./course-student-add-dialog.component.scss']
})
export class CourseStudentAddDialogComponent {
  private dialogRef = inject(MatDialogRef<CourseStudentAddDialogComponent>);
  private courseService = inject(CourseService);
  private snackBarService = inject(SnackBarService);
  private courseId: number = inject(MAT_DIALOG_DATA)
  private students: UserListRow[] = [];
  private selectedStudents: UserListRow[] = [];
  filteredStudents!: Observable<UserListRow[]>;
  selectedStudentsSubject = new BehaviorSubject(this.selectedStudents);
  selectedStudents$ = this.selectedStudentsSubject.asObservable();
  studentForm!: FormGroup;
  columnDefs = [
    new ColumnDefinition('Full name', 'fullName'),
    new ColumnDefinition('Email', 'email')
  ];

  get searchText() {
    return this.studentForm.get('searchText')!!;
  }

  ngOnInit() {
    this.courseService.getAddableStudents(this.courseId)
      .subscribe({
        next: students => {
          this.students = students;
        },
        error: error => {
          this.snackBarService.error('Something went wrong', error.error);
          this.dialogRef.close();
        }
      });

    this.setupForm();
  }

  onOptionSelected(event: MatAutocompleteSelectedEvent) {
    this.selectedStudents.push(event.option.value as UserListRow);
    this.selectedStudents
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

  onRemoveClick(student: UserListRow) {
    const index = this.selectedStudents.indexOf(student);
    this.selectedStudents.splice(index, 1);

    this.updateTable()
  }

  onOkClick() {
    const selectedStudentIds = this.selectedStudents
      .map(student => student.userId);

    this.courseService.addStudents(this.courseId, selectedStudentIds)
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

  displayFn(student: UserListRow): string {
    return student
      ? student.fullName + '(' + student.email + ')'
      : '';
  }

  private setupForm() {
    this.studentForm = new FormGroup({
      searchText: new FormControl('')
    });

    this.filteredStudents = this.searchText.valueChanges
      .pipe(
        startWith(''),
        map(value => this.filter(value || ''))
      );
  }

  private filter(value: string) {
    const filterValue = value.toString().toLowerCase();


    return this.students.filter(student => {


      return !this.selectedStudents.includes(student)
        && (student.fullName.toLowerCase().includes(filterValue)
          || student.email.toLowerCase().includes(filterValue));
    })
  }

  private updateTable() {
    this.selectedStudentsSubject.next(this.selectedStudents);
  }
}
