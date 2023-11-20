import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { CourseListComponent } from './course-list/course-list.component';
import { MatDividerModule } from "@angular/material/divider";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatOptionModule, MatRippleModule } from "@angular/material/core";
import { MatSortModule } from "@angular/material/sort";
import { MatTableModule } from "@angular/material/table";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatCardModule } from "@angular/material/card";
import { RouterLink, RouterLinkActive, RouterOutlet } from "@angular/router";
import { CourseCreateComponent } from './course-create/course-create.component';
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSelectModule } from "@angular/material/select";
import { AuthModule } from "../auth-module/auth.module";
import { ReactiveFormsModule } from "@angular/forms";
import {
  UniqueCourseNameAsyncValidatorDirective
} from './validators/unique-course-name/unique-course-name-async-validator.directive';
import { CourseEditComponent } from './course-edit/course-edit.component';
import { CourseDetailsComponent } from './course-details/course-details.component';
import { MatListModule } from "@angular/material/list";
import { MatSidenavModule } from "@angular/material/sidenav";
import { GroupListComponent } from './course-details/group-list/group-list.component';
import { GroupDetailsComponent } from './group/group-details/group-details.component';
import { MatMenuModule } from "@angular/material/menu";
import { MatToolbarModule } from "@angular/material/toolbar";
import { CourseToolbarComponent } from './course-details/course-toolbar/course-toolbar.component';
import { GroupCreateComponent } from './group/group-create/group-create.component';
import { GroupEditComponent } from './group/group-edit/group-edit.component';
import {
  UniqueGroupNameAsyncValidatorDirective
} from './validators/unique-group-name/unique-group-name-async-validator.directive';
import { MatTabsModule } from "@angular/material/tabs";
import { StudentListComponent } from './group/student/student-list/student-list.component';
import { TeacherListComponent } from './group/teacher/teacher-list/teacher-list.component';
import { ControlModule } from "../control-module/control.module";
import {
  GroupTeacherAddDialogComponent
} from './group/teacher/group-teacher-add-dialog/group-teacher-add-dialog.component';
import { MatDialogModule } from "@angular/material/dialog";
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { CourseTeacherAddDialogComponent } from './course-teacher-add-dialog/course-teacher-add-dialog.component';
import { CourseStudentAddDialogComponent } from './course-student-add-dialog/course-student-add-dialog.component';
import {
  GroupStudentAddDialogComponent
} from './group/student/group-student-add-dialog/group-student-add-dialog.component';
import { GroupAssignmentListComponent } from './group/assignment/group-assignment-list/group-assignment-list.component';
import {
  GroupAssignmentCreateDialogComponent
} from './group/assignment/group-assignment-create-dialog/group-assignment-create-dialog.component';


@NgModule({
  declarations: [
    CourseListComponent,
    CourseCreateComponent,
    UniqueCourseNameAsyncValidatorDirective,
    CourseEditComponent,
    CourseDetailsComponent,
    GroupListComponent,
    GroupDetailsComponent,
    CourseToolbarComponent,
    GroupCreateComponent,
    GroupEditComponent,
    UniqueGroupNameAsyncValidatorDirective,
    StudentListComponent,
    TeacherListComponent,
    GroupTeacherAddDialogComponent,
    CourseTeacherAddDialogComponent,
    CourseStudentAddDialogComponent,
    GroupStudentAddDialogComponent,
    GroupAssignmentCreateDialogComponent,
    GroupAssignmentListComponent
  ],
  imports: [
    CommonModule,
    MatDividerModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatRippleModule,
    MatSortModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    NgOptimizedImage,
    RouterLink,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatOptionModule,
    MatSelectModule,
    AuthModule,
    ReactiveFormsModule,
    MatListModule,
    MatSidenavModule,
    RouterLinkActive,
    RouterOutlet,
    MatMenuModule,
    MatToolbarModule,
    MatTabsModule,
    ControlModule,
    MatDialogModule,
    MatAutocompleteModule,
  ],
  exports: [
    CourseListComponent,
    CourseCreateComponent,
    CourseEditComponent,
    CourseDetailsComponent,
    GroupDetailsComponent,
    GroupCreateComponent,
    GroupEditComponent,
    StudentListComponent
  ]
})
export class CourseModule {
}
