export class AppointmentModel {
  readonly appointmentId!: number;
  readonly time!: string;
  readonly teacherName!: string;
  readonly teacherEmail!: string;
  readonly isAvailable!: boolean;
  readonly isMine!: boolean;
}