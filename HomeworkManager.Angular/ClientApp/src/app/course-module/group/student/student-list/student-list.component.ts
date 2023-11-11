import { Component, inject, Input, OnInit } from '@angular/core';
import { GroupService } from "../../../services/group.service";
import { ColumnDefinition, Pageable, PageableOptions, Role, UserListRow } from "../../../../shared-module";
import { NavigationItems } from "../../../../core-module";
import { AuthService } from "../../../../services";
import { MatDialog } from "@angular/material/dialog";
import { GroupStudentAddDialogComponent } from "../group-student-add-dialog/group-student-add-dialog.component";
import { Observable, startWith, Subject, switchMap } from "rxjs";

@Component({
  selector: 'hwm-student-list',
  templateUrl: './student-list.component.html',
  styleUrls: ['./student-list.component.scss']
})
export class StudentListComponent implements OnInit {
  private authService = inject(AuthService);
  private groupService = inject(GroupService);
  private dialog = inject(MatDialog);
  private students = new Subject<Pageable<UserListRow>>();
  protected readonly NavigationItems = NavigationItems;
  @Input() groupName!: string;
  isAdministrator = false;
  students$ = this.students.asObservable();
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
          return this.groupService.getStudents(this.groupName, options);
        })
      )
      .subscribe(students => {
        this.students.next(students);
      });
  }

  onAddClick() {
    this.dialog.open(GroupStudentAddDialogComponent, {
      data: this.groupName
    });
  }

  onRowRemove(student: UserListRow) {

  }
}
