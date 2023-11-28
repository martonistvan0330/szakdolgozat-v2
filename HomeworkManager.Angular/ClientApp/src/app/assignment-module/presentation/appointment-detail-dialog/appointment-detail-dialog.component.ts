import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { AppointmentModel } from "../../../shared-module";

@Component({
  selector: 'hwm-appointment-detail-dialog',
  templateUrl: './appointment-detail-dialog.component.html',
  styleUrls: ['./appointment-detail-dialog.component.scss']
})
export class AppointmentDetailDialogComponent {
  private dialogRef = inject(MatDialogRef<AppointmentDetailDialogComponent>);
  protected appointment: AppointmentModel = inject(MAT_DIALOG_DATA);

  onOkClick() {
    this.dialogRef.close(this.appointment.appointmentId)
  }

  onCancelClick() {
    this.dialogRef.close();
  }
}
