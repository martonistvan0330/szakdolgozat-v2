import { Component, inject, Input, OnDestroy, OnInit } from '@angular/core';
import { merge, Observable, Subject, switchMap } from "rxjs";
import {
  AssignmentListRowWithDate,
  ColumnDefinition,
  Pageable,
  Role,
  TableChangeOptions
} from "../../../../shared-module";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { AuthService } from "../../../../services";
import { GroupService } from "../../../services/group.service";
import { NavigationItems, SnackBarService } from "../../../../core-module";
import {
  GroupAssignmentCreateDialogComponent
} from "../group-assignment-create-dialog/group-assignment-create-dialog.component";
import { Router } from "@angular/router";

@Component({
  selector: 'hwm-group-assignment-list',
  templateUrl: './group-assignment-list.component.html',
  styleUrls: ['./group-assignment-list.component.scss']
})
export class GroupAssignmentListComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private snackBarService = inject(SnackBarService);
  private authService = inject(AuthService);
  private groupService = inject(GroupService);
  private dialog = inject(MatDialog);
  private assignments = new Subject<Pageable<AssignmentListRowWithDate>>();
  protected readonly NavigationItems = NavigationItems;
  @Input() groupName!: string;
  isAdministrator = false;
  @Input() isTeacher = false;
  assignments$ = this.assignments.asObservable();
  changeDataSource$!: Observable<string>;
  columnDefs = [
    new ColumnDefinition('Name', 'name', true),
    new ColumnDefinition('Deadline', 'deadline', true)
  ];

  ngOnInit() {
    if (this.isTeacher) {
      this.columnDefs = [
        new ColumnDefinition('Name', 'name', true),
        new ColumnDefinition('Deadline', 'deadline', true),
        new ColumnDefinition('Draft', 'isDraft')
      ];
    }

    this.authService.hasRole([Role.ADMINISTRATOR])
      .subscribe(isAdmin => {
        this.isAdministrator = isAdmin;
      });

    this.changeDataSource$ =
      merge(
        this.groupService.groupChanged$
      );
  }

  onOptionsSetup(options: Observable<TableChangeOptions<string>>) {
    options
      .pipe(
        switchMap(options => {
          return this.groupService.getAssignments(options.extras || '', options.pageableOptions);
        })
      )
      .subscribe(assignments => {
        this.assignments.next(assignments);
      });
  }

  onCreateClick() {
    const dialogRef: MatDialogRef<GroupAssignmentCreateDialogComponent, number> =
      this.dialog.open(GroupAssignmentCreateDialogComponent, {
        data: this.groupName
      });

    dialogRef.afterClosed()
      .subscribe(assignmentId => {
        if (assignmentId) {
          this.router
            .navigate([NavigationItems.assignmentDetails.navigationUrl, assignmentId])
            .then((success) => {
              if (success) {
                this.snackBarService.success('Assignment Created');
              }
            });
        }
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
