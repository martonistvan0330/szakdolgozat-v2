import { Component, inject, Input, OnInit } from '@angular/core';
import { AssignmentTypeId } from "../../../shared-module";
import { ActivatedRoute } from "@angular/router";
import { SnackBarService } from "../../../core-module";
import { AssignmentService } from "../../services/assignment.service";

@Component({
  selector: 'hwm-submission-details',
  templateUrl: './submission-details.component.html',
  styleUrls: ['./submission-details.component.scss']
})
export class SubmissionDetailsComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private snackBarService = inject(SnackBarService);
  private assignmentService = inject(AssignmentService);
  protected readonly AssignmentTypeId = AssignmentTypeId;
  @Input() assignmentId: number | null = null;
  assignmentTypeId: number | null = null;

  ngOnInit() {
    this.activatedRoute.paramMap
      .subscribe({
        next: params => {
          if (!this.assignmentId) {
            const assignmentId = params.get('assignmentId');

            if (assignmentId) {
              this.assignmentId = parseInt(assignmentId);
            }

          }

          this.assignmentService.getAssignmentTypeId(this.assignmentId ?? 0)
            .subscribe({
              next: assignmentTypeId => {
                this.assignmentTypeId = assignmentTypeId;
              }
            });
        },
        error: error => {
          this.snackBarService.error("Something went wrong");
        }
      });
  }
}
