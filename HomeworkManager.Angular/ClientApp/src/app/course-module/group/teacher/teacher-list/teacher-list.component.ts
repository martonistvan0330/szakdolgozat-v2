import { Component, inject, Input, OnDestroy, OnInit } from '@angular/core';
import { ColumnDefinition, Pageable, PageableOptions, Role, UserListRow } from "../../../../shared-module";
import { GroupService } from "../../../services/group.service";
import { NavigationItems } from "../../../../core-module";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { GroupTeacherAddDialogComponent } from "../group-teacher-add-dialog/group-teacher-add-dialog.component";
import { AuthService } from "../../../../services";
import { Observable, startWith, Subject, Subscription, switchMap } from "rxjs";
import { map } from "rxjs/operators";

@Component({
  selector: 'hwm-teacher-list',
  templateUrl: './teacher-list.component.html',
  styleUrls: ['./teacher-list.component.scss']
})
export class TeacherListComponent implements OnInit, OnDestroy {
  private authService = inject(AuthService);
  private groupService = inject(GroupService);
  private dialog = inject(MatDialog);
  private teachers = new Subject<Pageable<UserListRow>>();
  private teachersAddSubscription = new Subscription();
  protected readonly NavigationItems = NavigationItems;
  @Input() groupName!: string;
  isAdministrator = false;
  teachers$ = this.teachers.asObservable();
  columnDefs = [
    new ColumnDefinition('Full name', 'fullName', true),
    new ColumnDefinition('Email', 'email', true)
  ]

  ngOnInit() {
    this.authService.hasRole([Role.ADMINISTRATOR])
      .subscribe(isAdmin => {
        this.isAdministrator = isAdmin;
      });
  }

  onOptionsSetup(pageableOptions: Observable<PageableOptions>) {
    pageableOptions
      .pipe(
        startWith(new PageableOptions()),
        switchMap(options => {
          return this.groupService.getTeachers(this.groupName, options);
        })
      )
      .subscribe(teachers => {
        this.teachers.next(teachers);
      });
  }

  onAddClick() {
    const dialogRef: MatDialogRef<GroupTeacherAddDialogComponent, UserListRow[]> =
      this.dialog.open(GroupTeacherAddDialogComponent, {
        data: this.groupName
      });

    this.teachersAddSubscription = dialogRef.afterClosed()
      .pipe(
        map(selectedTeachers => {
          if (!selectedTeachers) {
            return [];
          }

          return selectedTeachers.map(teacher => teacher.userId);
        }),
        switchMap(selectedTeacherIds => {
          return this.groupService.addTeachers(this.groupName, selectedTeacherIds)
        })
      )
      .subscribe();
  }

  onRemoveClick(teacher: UserListRow) {

  }

  ngOnDestroy() {
    this.teachersAddSubscription.unsubscribe();
  }
}
