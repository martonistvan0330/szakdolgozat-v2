import { Component, inject, Input, OnInit } from '@angular/core';
import { SubmissionService } from "../../../services/submission.service";
import { saveAs } from "file-saver";
import { ActivatedRoute } from "@angular/router";
import { FileSubmissionModel } from "../../../../shared-module";
import { switchMap } from "rxjs";

@Component({
  selector: 'hwm-file-submission-details',
  templateUrl: './file-submission-details.component.html',
  styleUrls: ['./file-submission-details.component.scss']
})
export class FileSubmissionDetailsComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private submissionService = inject(SubmissionService);
  private userId: string | null = null;
  protected submission: FileSubmissionModel | null = null;
  protected file: File | null = null;
  @Input() assignmentId!: number;

  ngOnInit() {
    this.activatedRoute.paramMap
      .subscribe({
        next: params => {
          this.userId = params.get('userId');
          this.submissionService.getFileSubmission(this.assignmentId, this.userId)
            .subscribe({
              next: submissionModel => {
                this.setup(submissionModel);
              }
            });
        }
      })
  }

  onFileSelected(event: Event) {
    const files = (<HTMLInputElement>event.target).files;

    if (files) {
      this.file = files[0]
    }
  }

  onDownload() {
    this.submissionService.downloadFileSubmission(this.assignmentId)
      .subscribe({
        next: response => {
          if (this.submission) {
            saveAs(response, this.submission.fileName);
          }
        },
        error: error => {
          console.log(error);
        }
      })
  }

  onUpload() {
    if (this.file) {
      this.submissionService.uploadFileSubmission(this.assignmentId, this.file)
        .pipe(
          switchMap(() => {
            return this.submissionService.getFileSubmission(this.assignmentId, this.userId);
          })
        )
        .subscribe(submissionModel => {
          this.setup(submissionModel);
        });
    }
  }

  private setup(submissionModel: FileSubmissionModel | null) {
    this.submission = submissionModel

    // this.assignmentService.isTeacher(this.assignmentId)
    //   .subscribe({
    //     next: isTeacher => {
    //       this.isTeacher = isTeacher;
    //     }
    //   });
  }
}
