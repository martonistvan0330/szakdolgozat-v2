import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AssignmentDetailsComponent } from "./assignment-details/assignment-details.component";
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatTabsModule } from "@angular/material/tabs";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatCardModule } from "@angular/material/card";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatDatepickerModule } from "@angular/material/datepicker";
import {
  UniqueAssignmentNameAsyncValidatorDirective
} from './validators/unique-assignment-name/unique-assignment-name-async-validator.directive';
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatNativeDateModule } from "@angular/material/core";
import { MatSelectModule } from "@angular/material/select";
import { SubmissionDetailsComponent } from './submission/submission-details/submission-details.component';
import { SubmissionListComponent } from './submission/submission-list/submission-list.component';
import {
  TextSubmissionDetailsComponent
} from './submission/submission-details/text-submission-details/text-submission-details.component';
import {
  FileSubmissionDetailsComponent
} from './submission/submission-details/file-submission-details/file-submission-details.component';
import { MatDividerModule } from "@angular/material/divider";
import { ControlModule } from "../control-module/control.module";
import { AssignmentListComponent } from './assignment-list/assignment-list.component';


@NgModule({
  declarations: [
    AssignmentDetailsComponent,
    UniqueAssignmentNameAsyncValidatorDirective,
    SubmissionDetailsComponent,
    SubmissionListComponent,
    TextSubmissionDetailsComponent,
    FileSubmissionDetailsComponent,
    AssignmentListComponent,
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatTabsModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatSelectModule,
    MatDividerModule,
    ControlModule
  ],
  exports: [
    AssignmentDetailsComponent,
    SubmissionDetailsComponent,
    AssignmentListComponent
  ]
})
export class AssignmentModule {
}