import { Component, inject, Input, OnDestroy, OnInit } from '@angular/core';
import { ColumnDefinition, Pageable, Role, TableChangeOptions, UserListRow } from "../../../../shared-module";
import { GroupService } from "../../../services/group.service";
import { NavigationItems } from "../../../../core-module";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { GroupTeacherAddDialogComponent } from "../group-teacher-add-dialog/group-teacher-add-dialog.component";
import { AuthService } from "../../../../services";
import { filter, merge, Observable, Subject, Subscription, switchMap } from "rxjs";
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
  isCreator = false;
  teachers$ = this.teachers.asObservable();
  changeDataSource$!: Observable<string>;
  columnDefs = [
    new ColumnDefinition('Full name', 'fullName', true),
    new ColumnDefinition('Email', 'email', true)
  ]

  ngOnInit() {
    this.authService.hasRole([Role.ADMINISTRATOR])
      .subscribe(isAdmin => {
        this.isAdministrator = isAdmin;
      });

    this.groupService.isCreator(this.groupName)
      .subscribe({
        next: isCreator => {
          this.isCreator = isCreator;
        }
      });

    this.changeDataSource$ =
      merge(
        this.groupService.groupChanged$,
        this.groupService.teacherAdded$
      );
  }

  onOptionsSetup(options: Observable<TableChangeOptions<string>>) {
    options
      .pipe(
        switchMap(options => {
          return this.groupService.getTeachers(options.extras || '', options.pageableOptions);
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
        filter(selectedTeachers => {
          if (!selectedTeachers) {
            return false;
          }
          return selectedTeachers.length > 0;
        }),
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
