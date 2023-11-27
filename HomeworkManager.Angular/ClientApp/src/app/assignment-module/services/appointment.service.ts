import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../services";
import { NewAppointment } from "../../shared-module";

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private authApiClient = inject(AuthorizedApiClientService);

  createAppointment(newAppointment: NewAppointment) {
    return this.authApiClient.post<void>('Appointment', newAppointment);
  }
}
