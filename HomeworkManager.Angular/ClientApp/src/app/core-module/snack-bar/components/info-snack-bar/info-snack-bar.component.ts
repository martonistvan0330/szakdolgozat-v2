import { Component, inject } from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBarRef } from "@angular/material/snack-bar";
import { SnackBarConfig } from "../../config/snack-bar-config";

@Component({
  selector: 'hwm-info-snack-bar',
  templateUrl: './info-snack-bar.component.html',
  styleUrls: ['./info-snack-bar.component.scss']
})
export class InfoSnackBarComponent {
  snackbarRef: MatSnackBarRef<SnackBarConfig> = inject(MatSnackBarRef);
  data: SnackBarConfig = inject(MAT_SNACK_BAR_DATA);
}
