import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { SuccessSnackBarComponent } from './snack-bar/components/success-snack-bar/success-snack-bar.component';
import { MatButtonModule } from "@angular/material/button";
import { MatIconModule } from "@angular/material/icon";
import { MatProgressBarModule } from "@angular/material/progress-bar";
import { ErrorSnackBarComponent } from './snack-bar/components/error-snack-bar/error-snack-bar.component';


@NgModule({
  declarations: [
    SuccessSnackBarComponent,
    ErrorSnackBarComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MatSnackBarModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule
  ]
})
export class CoreModule {
}
