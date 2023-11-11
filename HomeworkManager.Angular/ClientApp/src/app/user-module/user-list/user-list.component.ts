import { Component, inject } from '@angular/core';
import { ColumnDefinition, Pageable, PageableOptions, UserListRow } from "../../shared-module";
import { UserService } from "../services/user.service";
import { NavigationItems } from "../../core-module";
import { Router } from "@angular/router";
import { Observable, Subject, switchMap } from "rxjs";

@Component({
  selector: 'hwm-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent {
  private router = inject(Router);
  private userService = inject(UserService);
  private users = new Subject<Pageable<UserListRow>>();
  protected readonly NavigationItems = NavigationItems;
  users$ = this.users.asObservable();
  columnDefs = [
    new ColumnDefinition('ID', 'userId', true),
    new ColumnDefinition('Full name', 'fullName', true),
    new ColumnDefinition('Username', 'username', true),
    new ColumnDefinition('Email', 'email', true),
    new ColumnDefinition('Roles', 'roles', false),
  ]

  onOptionsSetup(pageableOptions: Observable<PageableOptions>) {
    pageableOptions
      .pipe(
        switchMap(options => {
          return this.userService.getUserList(options);
        })
      )
      .subscribe(users => {
        this.users.next(users);
      });
  }

  async onRowClick(row: UserListRow) {
    await this.router.navigate([NavigationItems.userDetail.navigationUrl, row.userId]);
  }
}
