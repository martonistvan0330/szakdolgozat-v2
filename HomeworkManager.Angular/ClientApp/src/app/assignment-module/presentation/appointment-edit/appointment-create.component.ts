import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Errors, NewAppointment } from "../../../shared-module";
import { SnackBarService } from "../../../core-module";
import { AppointmentService } from "../../services/appointment.service";

@Component({
  selector: 'hwm-appointment-edit',
  templateUrl: './appointment-create.component.html',
  styleUrls: ['./appointment-create.component.scss']
})
export class AppointmentCreateComponent implements OnInit {
  private snackBarService = inject(SnackBarService);
  private appointmentService = inject(AppointmentService);
  protected minDate = new Date();
  protected appointmentForm!: FormGroup;
  protected presentationLengthError = Errors.NoError;
  protected readonly Errors = Errors;
  @Input() assignmentId!: number;
  @Output() created = new EventEmitter<void>;

  protected get date() {
    return this.appointmentForm.get('date')!!;
  }

  protected get presentationLength() {
    return this.appointmentForm.get('presentationLength')!!;
  }

  protected get startTime() {
    return this.appointmentForm.get('startTime')!!;
  }

  protected get endTime() {
    return this.appointmentForm.get('endTime')!!;
  }

  ngOnInit() {
    this.formSetup()

    this.presentationLengthControlSetup();
  }

  onAddAppointmentClick() {
    if (this.appointmentForm.invalid) {
      return
    }

    const newAppointment = new NewAppointment();
    newAppointment.assignmentId = this.assignmentId;
    if (typeof this.date.value == 'string') {
      newAppointment.date = this.date.value;
    } else {
      const deadline = new Date(this.date.value)
      deadline.setDate(this.date.value.getDate() + 1);
      newAppointment.date = deadline.toISOString();
    }
    newAppointment.presentationLength = this.presentationLength.value;
    newAppointment.startTime = this.startTime.value;
    newAppointment.endTime = this.endTime.value;

    this.appointmentService.createAppointment(newAppointment)
      .subscribe({
        next: () => {
          this.created.emit();
        },
        error: error => {
          this.snackBarService.error("Appointment creation failed", error.error);
        }
      })
  }

  private formSetup() {
    const date = new Date();
    date.setHours(0);
    date.setMinutes(0);

    this.appointmentForm = new FormGroup({
      date: new FormControl({ value: date, disabled: true }, {
        validators: [Validators.required]
      }),
      presentationLength: new FormControl(20, {
        validators: [Validators.required, Validators.min(5), Validators.max(90), Validators.pattern("\\d*")]
      }),
      startTime: new FormControl('9:00', {
        validators: [Validators.required]
      }),
      endTime: new FormControl('17:00', {
        validators: [Validators.required]
      })
    });
  }

  private presentationLengthControlSetup() {
    this.presentationLength.statusChanges
      .subscribe(status => {
        if (status === 'VALID') {
          this.presentationLengthError = Errors.NoError;
        }

        if (status === 'INVALID') {
          if (this.presentationLength.errors?.['required']) {
            this.presentationLengthError = Errors.Required;
          } else if (this.presentationLength.errors?.['min']) {
            this.presentationLengthError = Errors.Min;
          } else if (this.presentationLength.errors?.['pattern']) {
            this.presentationLengthError = Errors.Pattern;
          }
        }
      });
  }

  private getMinutes(time: string) {
    return parseInt(time.split(":")[1]);
  }

  private getAllMinutes(time: string) {
    const parts = time.split(":");
    const hours = parseInt(parts[0]);
    const minutes = parseInt(parts[1]);
    return hours * 60 + minutes;
  }

  private getTime(allMinutes: number) {
    const hours = Math.floor(allMinutes / 60);
    const minutes = allMinutes % 60;
    return `${hours}:${minutes}`
  }
}
