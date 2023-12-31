import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { SuccessSnackBarComponent } from './snack-bar/components/success-snack-bar/success-snack-bar.component';
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatProgressBarModule } from "@angular/material/progress-bar";
import { ErrorSnackBarComponent } from './snack-bar/components/error-snack-bar/error-snack-bar.component';
import { ContainsDigitValidatorDirective } from './validation/contains-digit/contains-digit-validator.directive';
import {
  ContainsLowerCaseValidatorDirective
} from './validation/contains-lower-case/contains-lower-case-validator.directive';
import {
  ContainsUpperCaseValidatorDirective
} from './validation/contains-upper-case/contains-upper-case-validator.directive';


@NgModule({
  declarations: [
    SuccessSnackBarComponent,
    ErrorSnackBarComponent,
    ContainsDigitValidatorDirective,
    ContainsLowerCaseValidatorDirective,
    ContainsUpperCaseValidatorDirective
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatSnackBarModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule
  ],
  exports: [
    ContainsDigitValidatorDirective,
    ContainsLowerCaseValidatorDirective,
    ContainsUpperCaseValidatorDirective
  ]
})
export class CoreModule {
}
