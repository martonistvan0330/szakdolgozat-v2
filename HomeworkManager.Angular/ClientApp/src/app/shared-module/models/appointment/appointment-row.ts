import { AppointmentModel } from "./appointment-model";

export class AppointmentRow {
  date!: string;
  readonly appointmentModels: AppointmentModel[] = [];
}