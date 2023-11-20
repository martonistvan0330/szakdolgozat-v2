import { Component, inject, OnDestroy } from '@angular/core';
import { Router } from "@angular/router";
import { NavigationItems, SnackBarService } from "../../core-module";
import { AuthService } from "../../services";
import { MatDialog } from "@angular/material/dialog";
import { Observable, Subject, switchMap } from "rxjs";
import { AssignmentListRowWithDate, ColumnDefinition, Pageable, TableChangeOptions } from "../../shared-module";
import { AssignmentService } from "../services/assignment.service";

@Component({
  selector: 'hwm-assignment-list',
  templateUrl: './assignment-list.component.html',
  styleUrls: ['./assignment-list.component.scss']
})
export class AssignmentListComponent implements OnDestroy {
  private router = inject(Router);
  private snackBarService = inject(SnackBarService);
  private authService = inject(AuthService);
  private assignmentService = inject(AssignmentService);
  private dialog = inject(MatDialog);
  private assignments = new Subject<Pageable<AssignmentListRowWithDate>>();
  assignments$ = this.assignments.asObservable();
  columnDefs = [
    new ColumnDefinition('Name', 'name', true),
    new ColumnDefinition('Deadline', 'deadline', true)
  ];

  onOptionsSetup(options: Observable<TableChangeOptions<void>>) {
    options
      .pipe(
        switchMap(options => {
          return this.assignmentService.getAssignments(options.pageableOptions);
        })
      )
      .subscribe(assignments => {
        this.assignments.next(assignments);
      });
  }

  onRowClick(assignment: AssignmentListRowWithDate) {
    this.router
      .navigate([NavigationItems.assignmentDetails.navigationUrl, assignment.assignmentId])
      .then();
  }

  ngOnDestroy() {
  }
}
