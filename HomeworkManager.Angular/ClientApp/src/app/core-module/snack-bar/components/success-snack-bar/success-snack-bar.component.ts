import { Component, inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from "@angular/material/snack-bar";
import { SnackBarConfig } from "../../config/snack-bar-config";

@Component({
  selector: 'hwm-success-snack-bar',
  templateUrl: './success-snack-bar.component.html',
  styleUrls: ['./success-snack-bar.component.scss']
})
export class SuccessSnackBarComponent {
  snackbarRef: MatSnackBarRef<SnackBarConfig> = inject(MatSnackBarRef);
  data: SnackBarConfig = inject(MAT_SNACK_BAR_DATA);
}
