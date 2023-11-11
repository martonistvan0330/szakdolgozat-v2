import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ColumnDefinition, UserListRow } from "../../shared-module";
import { Observable } from "rxjs";
import { CourseService } from "../services/course.service";

@Component({
  selector: 'hwm-course-teacher-add-dialog',
  templateUrl: './course-teacher-add-dialog.component.html',
  styleUrls: ['./course-teacher-add-dialog.component.scss']
})
export class CourseTeacherAddDialogComponent {
  private courseService = inject(CourseService);
  private courseId: number = inject(MAT_DIALOG_DATA);
  protected readonly UserListRow = UserListRow;
  dataSource!: Observable<UserListRow[]>;
  columnDefs = [
    new ColumnDefinition('Full name', 'fullName'),
    new ColumnDefinition('Email', 'email')
  ];

  ngOnInit() {
    this.dataSource = this.courseService.getAddableTeachers(this.courseId);
  }
}
