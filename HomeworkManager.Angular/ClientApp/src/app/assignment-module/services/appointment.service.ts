import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import { AppointmentRow, NewAppointment } from "../../shared-module";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private authApiClient = inject(AuthorizedApiClientService);

  getAppointments(assignmentId: number) {
    return this.authApiClient.get<AppointmentRow[]>('Appointment/' + assignmentId)
      .pipe(
        map(appointmentRows => {
          appointmentRows = appointmentRows.map(appointmentRow => {
            appointmentRow.date = appointmentRow.date.split('T')[0];
            return appointmentRow
          })
          return appointmentRows
        })
      );
  }

  createAppointment(newAppointment: NewAppointment) {
    return this.authApiClient.post<void>('Appointment', newAppointment);
  }

  signUp(appointmentId: number) {
    return this.authApiClient.patch<void>('Appointment/' + appointmentId, {})
  }
}
