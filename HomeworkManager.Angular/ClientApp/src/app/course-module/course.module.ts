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
import { GroupDetailsComponent } from './course-details/group-details/group-details.component';
import { MatMenuModule } from "@angular/material/menu";
import { MatToolbarModule } from "@angular/material/toolbar";
import { CourseToolbarComponent } from './course-details/course-toolbar/course-toolbar.component';


@NgModule({
  declarations: [
    CourseListComponent,
    CourseCreateComponent,
    UniqueCourseNameAsyncValidatorDirective,
    CourseEditComponent,
    CourseDetailsComponent,
    GroupListComponent,
    GroupDetailsComponent,
    CourseToolbarComponent
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
  ],
  exports: [
    CourseListComponent,
    CourseCreateComponent,
    CourseEditComponent,
    CourseDetailsComponent,
    GroupDetailsComponent
  ]
})
export class CourseModule {
}
