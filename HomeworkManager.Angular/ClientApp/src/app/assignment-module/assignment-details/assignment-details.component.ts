import { Component, inject, OnInit } from '@angular/core';
import { AssignmentModel, AssignmentType, Errors } from "../../shared-module";
import { ActivatedRoute } from "@angular/router";
import { AssignmentService } from "../services/assignment.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import {
  uniqueAssignmentNameAsyncValidator
} from "../validators/unique-assignment-name/unique-assignment-name-async-validator.directive";
import { UpdatedAssignment } from "../../shared-module/models/assignment/updated-assignment";
import { MatCheckboxChange } from "@angular/material/checkbox";
import { SnackBarService } from "../../core-module";

@Component({
  selector: 'hwm-assignment-details',
  templateUrl: './assignment-details.component.html',
  styleUrls: ['./assignment-details.component.scss']
})
export class AssignmentDetailsComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private snackBarService = inject(SnackBarService);
  private assignmentService = inject(AssignmentService);
  protected readonly Errors = Errors;
  assignmentTypes: AssignmentType[] = [];
  assignment!: AssignmentModel;
  assignmentForm!: FormGroup;
  presentationRequired = false;
  minDate = new Date();
  isTeacher = false;
  isCreator = false;
  nameError = Errors.NoError;

  constructor() {
    const now = new Date();
    this.minDate.setDate(now.getDate() + 14);
  }

  get courseName() {
    return this.assignmentForm.get('courseName')!!;
  }

  get groupName() {
    return this.assignmentForm.get('groupName')!!;
  }

  get name() {
    return this.assignmentForm.get('name')!!;
  }

  get description() {
    return this.assignmentForm.get('description')!!;
  }

  get deadline() {
    return this.assignmentForm.get('deadline')!!;
  }

  get assignmentType() {
    return this.assignmentForm.get('assignmentType')!!;
  }


  ngOnInit() {
    this.activatedRoute.data
      .subscribe(({ assignment }) => {
        const assignmentModel = assignment as AssignmentModel;
        this.setup(assignmentModel);
      });
  }

  save() {
    const updatedAssignment = new UpdatedAssignment();
    updatedAssignment.assignmentId = this.assignment.assignmentId;
    updatedAssignment.name = this.name.value;
    updatedAssignment.description = this.description.value;
    if (typeof this.deadline.value == 'string') {
      updatedAssignment.deadline = this.deadline.value;
    } else {
      const asd = this.deadline.value.getDate();
      const deadline = new Date(this.deadline.value)
      deadline.setDate(this.deadline.value.getDate() + 1);
      updatedAssignment.deadline = deadline.toISOString();
    }
    updatedAssignment.presentationRequired = this.presentationRequired;
    updatedAssignment.assignmentTypeId = this.assignmentType.value;

    this.assignmentService.updateAssignment(updatedAssignment)
      .subscribe({
        next: () => {
          this.snackBarService.success("Assignment updated");
        },
        error: error => {
          this.snackBarService.error("Update failed", error.error);
        }
      });
  }

  publish() {
    this.assignmentService.publishAssignment(this.assignment.assignmentId)
      .subscribe({
        next: () => {
          this.assignment.isDraft = false;

          this.name.disable();
          this.description.disable();
          this.deadline.disable();
          this.assignmentType.disable();
        },
        error: error => {
          this.snackBarService.error("Publish failed", error.error);
        }
      })
  }

  presentationRequiredChanged(value: MatCheckboxChange) {
    this.presentationRequired = value.checked;
  }

  private setup(assignmentModel: AssignmentModel) {
    this.assignmentService.getTypes()
      .subscribe(assignmentTypes => {
        this.assignmentTypes = assignmentTypes;
      });

    this.assignment = assignmentModel

    this.assignmentService.isCreator(this.assignment.assignmentId)
      .subscribe({
        next: isCreator => {
          this.isCreator = isCreator;

          if (isCreator && this.assignment.isDraft) {
            this.name.enable();
            this.description.enable();
            this.deadline.enable();
            this.assignmentType.enable();
          }
        }
      });

    this.assignmentService.isTeacher(this.assignment.assignmentId)
      .subscribe({
        next: isTeacher => {
          this.isTeacher = isTeacher;
        }
      });

    if (assignmentModel) {
      this.formSetup()

      this.nameControlSetup();
    }
  }

  private formSetup() {
    this.presentationRequired = this.assignment.presentationRequired;

    this.assignmentForm = new FormGroup({
      courseName: new FormControl({ value: this.assignment.courseName, disabled: true }),
      groupName: new FormControl({ value: this.assignment.groupName, disabled: true }),
      name: new FormControl({ value: this.assignment.name, disabled: true }, {
        validators: [Validators.required],
        asyncValidators: [
          uniqueAssignmentNameAsyncValidator(
            this.assignmentService,
            this.assignment.courseId,
            this.assignment.groupName,
            this.assignment.name
          )
        ],
        updateOn: 'change'
      }),
      description: new FormControl({ value: this.assignment.description, disabled: true }),
      deadline: new FormControl({ value: this.assignment.deadline, disabled: true }, {
        validators: [Validators.required]
      }),
      assignmentType: new FormControl({ value: this.assignment.assignmentTypeId, disabled: true }, {
        validators: [Validators.required]
      })
    });
  }

  private nameControlSetup() {
    this.name.statusChanges
      .subscribe(() => {
        if (this.name.status === 'VALID') {
          this.nameError = Errors.NoError;
        }

        if (this.name.status === 'INVALID') {
          if (this.name.errors?.['required']) {
            this.nameError = Errors.Required;
          } else if (this.name.errors?.['uniqueName']) {
            this.nameError = Errors.Unique;
          }
        }
      });
  }
}
