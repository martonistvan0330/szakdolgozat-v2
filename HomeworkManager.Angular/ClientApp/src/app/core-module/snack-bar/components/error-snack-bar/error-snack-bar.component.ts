import { Component, inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from "@angular/material/snack-bar";
import { SnackBarConfig } from "../../config/snack-bar-config";

@Component({
  selector: 'hwm-error-snack-bar',
  templateUrl: './error-snack-bar.component.html',
  styleUrls: ['./error-snack-bar.component.scss']
})
export class ErrorSnackBarComponent {
  snackbarRef: MatSnackBarRef<SnackBarConfig> = inject(MatSnackBarRef);
  data: SnackBarConfig = inject(MAT_SNACK_BAR_DATA);
}
