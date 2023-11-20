import { Component, inject, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { NavigationItems } from "../../../core-module";
import { merge, Observable, Subject, switchMap } from "rxjs";
import { ColumnDefinition, Pageable, SubmissionListRowWithDate, TableChangeOptions } from "../../../shared-module";
import { SubmissionService } from "../../services/submission.service";

@Component({
  selector: 'hwm-submission-list',
  templateUrl: './submission-list.component.html',
  styleUrls: ['./submission-list.component.scss']
})
export class SubmissionListComponent implements OnInit, OnDestroy {
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private submissionService = inject(SubmissionService);
  private submissions = new Subject<Pageable<SubmissionListRowWithDate>>();
  protected readonly NavigationItems = NavigationItems;
  @Input() assignmentId!: number;
  submissions$ = this.submissions.asObservable();
  changeDataSource$!: Observable<number>;
  columnDefs = [
    new ColumnDefinition('Submitted by', 'studentName', true),
    new ColumnDefinition('Submitted at', 'submittedAt', true)
  ];

  ngOnInit() {
    this.changeDataSource$ = merge();
  }

  onOptionsSetup(options: Observable<TableChangeOptions<number>>) {
    options
      .pipe(
        switchMap(options => {
          return this.submissionService.getSubmissions(this.assignmentId, options.pageableOptions);
        })
      )
      .subscribe(submissions => {
        return this.submissions.next(submissions);
      });
  }

  onRowClick(submission: SubmissionListRowWithDate) {
    this.router
      .navigate([NavigationItems.submissionDetails.navigationUrl, submission.studentId], {
        relativeTo: this.activatedRoute
      })
      .then();
  }

  ngOnDestroy() {
  }
}
