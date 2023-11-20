import { Component, inject, Input, OnInit } from '@angular/core';
import { SubmissionService } from "../../../services/submission.service";
import { ActivatedRoute } from "@angular/router";
import { TextSubmissionModel, UpdatedTextSubmission } from "../../../../shared-module";
import { AssignmentService } from "../../../services/assignment.service";
import { FormControl, FormGroup } from "@angular/forms";
import { SnackBarService } from "../../../../core-module";

@Component({
  selector: 'hwm-text-submission-details',
  templateUrl: './text-submission-details.component.html',
  styleUrls: ['./text-submission-details.component.scss']
})
export class TextSubmissionDetailsComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private snackBarService = inject(SnackBarService);
  private assignmentService = inject(AssignmentService);
  private submissionService = inject(SubmissionService);
  @Input() assignmentId!: number;
  submission: TextSubmissionModel | null = null;
  submissionForm!: FormGroup;
  actionsEnabled = true;
  isTeacher = false;

  get answer() {
    return this.submissionForm.get('answer')!!;
  }

  ngOnInit() {
    this.formSetupDefault();

    this.activatedRoute.paramMap
      .subscribe({
        next: params => {
          const userId = params.get('userId');
          this.submissionService.getTextSubmission(this.assignmentId, userId)
            .subscribe({
              next: submissionModel => {
                this.setup(submissionModel);
              }
            });
        }
      })
  }

  save() {
    const updatedTextSubmission = new UpdatedTextSubmission;

    updatedTextSubmission.assignmentId = this.assignmentId;
    updatedTextSubmission.answer = this.answer.value;

    this.submissionService.updateTextSubmission(updatedTextSubmission)
      .subscribe({
        next: submissionId => {
          this.snackBarService.success("Answer updated");
        }
      });
  }

  submit() {
    this.submissionService.submit(this.assignmentId)
      .subscribe({
        next: submissionId => {
          this.snackBarService.success("Answer submitted");
        }
      });
  }

  private setup(submissionModel: TextSubmissionModel | null) {
    this.submission = submissionModel

    this.assignmentService.isTeacher(this.assignmentId)
      .subscribe({
        next: isTeacher => {
          this.isTeacher = isTeacher;
        }
      });

    if (submissionModel) {
      this.actionsEnabled = submissionModel.isDraft;
      this.formSetup(!this.actionsEnabled)
    }
  }

  private formSetup(readonly: boolean) {
    this.submissionForm = new FormGroup({
      answer: new FormControl({ value: this.submission!.answer, disabled: readonly })
    });
  }

  private formSetupDefault() {
    this.submissionForm = new FormGroup({
      answer: new FormControl('')
    });
  }
}
