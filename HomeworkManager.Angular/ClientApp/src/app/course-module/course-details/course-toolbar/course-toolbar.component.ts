import { Component, EventEmitter, inject, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { NavigationItems } from "../../../core-module";
import { CourseModel, Role, UserListRow } from "../../../shared-module";
import { CourseService } from "../../services/course.service";
import { AuthService } from "../../../services";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { CourseStudentAddDialogComponent } from "../../course-student-add-dialog/course-student-add-dialog.component";
import { CourseTeacherAddDialogComponent } from "../../course-teacher-add-dialog/course-teacher-add-dialog.component";
import { filter, Subscription, switchMap } from "rxjs";
import { map } from "rxjs/operators";

@Component({
  selector: 'hwm-course-toolbar',
  templateUrl: './course-toolbar.component.html',
  styleUrls: ['./course-toolbar.component.scss']
})
export class CourseToolbarComponent implements OnInit, OnDestroy {
  private courseService = inject(CourseService);
  private authService = inject(AuthService);
  private dialog = inject(MatDialog);
  private teachersAddSubscription = new Subscription();
  private studentsAddSubscription = new Subscription();
  protected readonly NavigationItems = NavigationItems;
  @Input() course!: CourseModel;
  @Input() isMobile: boolean | null = false;
  @Output() toggleNavbar = new EventEmitter<void>();
  isAdministrator = false
  isCreator = false;
  isTeacher = false;

  ngOnInit() {
    this.authService.hasRole([Role.ADMINISTRATOR])
      .subscribe(isAdmin => {
        this.isAdministrator = isAdmin;
      });

    this.courseService.isCreator(this.course.courseId)
      .subscribe({
        next: isCreator => {
          this.isCreator = isCreator;
        }
      });

    this.courseService.isTeacher(this.course.courseId)
      .subscribe({
        next: isTeacher => {
          this.isTeacher = isTeacher;
        }
      });
  }

  onAddTeachersClick() {
    const dialogRef: MatDialogRef<CourseTeacherAddDialogComponent, UserListRow[]> =
      this.dialog.open(CourseTeacherAddDialogComponent, {
        data: this.course.courseId
      });

    this.teachersAddSubscription = dialogRef.afterClosed()
      .pipe(
        filter(selectedTeachers => {
          if (!selectedTeachers) {
            return false;
          }
          return selectedTeachers.length > 0;
        }),
        map(selectedTeachers => {
          if (!selectedTeachers) {
            return [];
          }

          return selectedTeachers.map(teacher => teacher.userId);
        }),
        switchMap(selectedTeacherIds => {
          return this.courseService.addTeachers(this.course.courseId, selectedTeacherIds)
        })
      )
      .subscribe();
  }

  onAddStudentsClick() {
    const dialogRef: MatDialogRef<CourseStudentAddDialogComponent, UserListRow[]> =
      this.dialog.open(CourseStudentAddDialogComponent, {
        data: this.course.courseId
      });

    this.studentsAddSubscription = dialogRef.afterClosed()
      .pipe(
        filter(selectedStudents => {
          if (!selectedStudents) {
            return false;
          }
          return selectedStudents.length > 0;
        }),
        map(selectedStudents => {
          if (!selectedStudents) {
            return [];
          }

          return selectedStudents.map(student => student.userId);
        }),
        switchMap(selectedStudentIds => {
          return this.courseService.addStudents(this.course.courseId, selectedStudentIds)
        })
      )
      .subscribe();
  }

  onToggle() {
    this.toggleNavbar.emit();
  }

  ngOnDestroy() {
    this.teachersAddSubscription.unsubscribe();
    this.studentsAddSubscription.unsubscribe();
  }
}
