import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { NavigationItems } from "../../../core-module";
import { CourseModel, Role } from "../../../shared-module";
import { CourseService } from "../../services/course.service";
import { AuthService } from "../../../services";
import { MatDialog } from "@angular/material/dialog";
import { CourseStudentAddDialogComponent } from "../../course-student-add-dialog/course-student-add-dialog.component";
import { CourseTeacherAddDialogComponent } from "../../course-teacher-add-dialog/course-teacher-add-dialog.component";

@Component({
  selector: 'hwm-course-toolbar',
  templateUrl: './course-toolbar.component.html',
  styleUrls: ['./course-toolbar.component.scss']
})
export class CourseToolbarComponent implements OnInit {
  private courseService = inject(CourseService);
  private authService = inject(AuthService);
  private dialog = inject(MatDialog);
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
    this.dialog.open(CourseTeacherAddDialogComponent, {
      data: this.course.courseId
    });
  }

  onAddStudentsClick() {
    this.dialog.open(CourseStudentAddDialogComponent, {
      data: this.course.courseId,
      panelClass: 'my-class',
    });
  }

  onToggle() {
    this.toggleNavbar.emit();
  }
}
