import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { CourseService } from "../services/course.service";
import { ColumnDefinition, UserListRow } from "../../shared-module";
import { Observable } from "rxjs";

@Component({
  selector: 'hwm-course-student-add-dialog',
  templateUrl: './course-student-add-dialog.component.html',
  styleUrls: ['./course-student-add-dialog.component.scss']
})
export class CourseStudentAddDialogComponent {
  private courseService = inject(CourseService);
  private courseId: number = inject(MAT_DIALOG_DATA);
  protected readonly UserListRow = UserListRow;
  dataSource!: Observable<UserListRow[]>;
  columnDefs = [
    new ColumnDefinition('Full name', 'fullName'),
    new ColumnDefinition('Email', 'email')
  ];

  ngOnInit() {
    this.dataSource = this.courseService.getAddableStudents(this.courseId);
  }
}
