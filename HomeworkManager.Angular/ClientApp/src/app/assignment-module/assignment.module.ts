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


@NgModule({
  declarations: [
    AssignmentDetailsComponent,
    UniqueAssignmentNameAsyncValidatorDirective,
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
    MatSelectModule
  ],
  exports: [
    AssignmentDetailsComponent
  ]
})
export class AssignmentModule {
}
