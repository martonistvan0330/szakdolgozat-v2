import { Component, inject, Input, OnInit } from '@angular/core';
import { ColumnDefinition, Pageable, PageableOptions, Role, UserListRow } from "../../../../shared-module";
import { GroupService } from "../../../services/group.service";
import { NavigationItems } from "../../../../core-module";
import { MatDialog } from "@angular/material/dialog";
import { GroupTeacherAddDialogComponent } from "../group-teacher-add-dialog/group-teacher-add-dialog.component";
import { AuthService } from "../../../../services";
import { Observable, startWith, Subject, switchMap } from "rxjs";

@Component({
  selector: 'hwm-teacher-list',
  templateUrl: './teacher-list.component.html',
  styleUrls: ['./teacher-list.component.scss']
})
export class TeacherListComponent implements OnInit {
  private authService = inject(AuthService);
  private groupService = inject(GroupService);
  private dialog = inject(MatDialog);
  private teachers = new Subject<Pageable<UserListRow>>();
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
    this.dialog.open(GroupTeacherAddDialogComponent, {
      data: this.groupName
    });
  }

  onRemoveClick(teacher: UserListRow) {

  }
}
