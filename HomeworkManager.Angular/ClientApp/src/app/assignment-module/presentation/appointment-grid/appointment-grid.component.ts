import { Component, EventEmitter, Inject, inject, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { AppointmentService } from "../../services/appointment.service";
import { SnackBarService } from "../../../core-module";
import { AppointmentModel, AppointmentRow } from "../../../shared-module";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { AppointmentDetailDialogComponent } from "../appointment-detail-dialog/appointment-detail-dialog.component";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { AuthService } from "../../../services";

@Component({
  selector: 'hwm-appointment-grid',
  templateUrl: './appointment-grid.component.html',
  styleUrls: ['./appointment-grid.component.scss']
})
export class AppointmentGridComponent implements OnInit, OnDestroy {
  private dialog = inject(MatDialog);
  private snackBarService = inject(SnackBarService);
  private authService = inject(AuthService);
  private appointmentService = inject(AppointmentService);
  private readonly connection: HubConnection;
  appointmentRows: AppointmentRow[] = [];
  @Input() assignmentId!: number;
  @Output() addClick = new EventEmitter<void>()

  constructor(@Inject('BASE_URL') private readonly baseUrl: string) {
    this.connection = new HubConnectionBuilder()
      .configureLogging(LogLevel.Information)
      .withUrl(baseUrl + 'appointment')
      .build();
  }

  ngOnInit() {
    this.connection.start()
      .then(() => {
        this.connection.on('Refresh', () => {
          this.appointmentService.getAppointments(this.assignmentId)
            .subscribe({
              next: appointmentRows => {
                this.appointmentRows = appointmentRows;
              },
              error: error => {
                this.snackBarService.error("Something went wrong", error.error);
              }
            });
        });

        this.connection.invoke('JoinRoom', this.assignmentId)
          .then()
          .catch(() => {
            this.authService.authenticate();

            this.connection.invoke('JoinRoom', this.assignmentId).then();
          });
      })
      .catch(error => {
        console.log(error);
      });
  }

  onAppointmentClick(appointment: AppointmentModel) {
    const dialogRef: MatDialogRef<AppointmentDetailDialogComponent, number> =
      this.dialog.open(AppointmentDetailDialogComponent, {
        data: appointment
      });

    dialogRef.afterClosed()
      .subscribe(appointmentId => {
        if (appointmentId) {
          this.appointmentService.signUp(appointmentId)
            .subscribe({
              next: () => {
                this.snackBarService.success('Successful sign up');
              },
              error: error => {
                this.snackBarService.error('Appointment sign up failed!', error.error);
              }
            });
        }
      });
  }

  onEditClick() {
    this.addClick.emit();
  }

  onAssignAllClick() {
    this.appointmentService.assignStudents(this.assignmentId, false)
      .subscribe({
        next: () => {
          this.snackBarService.success('Successful assignments');
        }
      });
  }

  onAssignSubmittedClick() {
    this.appointmentService.assignStudents(this.assignmentId, true)
      .subscribe({
        next: () => {
          this.snackBarService.success('Successful assignments');
        }
      });
  }

  async ngOnDestroy() {
    await this.connection.stop();
  }
}
