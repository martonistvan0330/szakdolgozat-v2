import { Component, inject, ViewChild } from '@angular/core';
import { catchError, merge, Observable, of, startWith, switchMap } from "rxjs";
import { UserListRow } from "../../shared-module";
import { MatSort } from "@angular/material/sort";
import { MatPaginator } from "@angular/material/paginator";
import { map } from "rxjs/operators";
import { UserService } from "../services/user.service";
import { NavigationItems } from "../../core-module";

@Component({
  selector: 'hwm-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent {
  private userService = inject(UserService);
  protected readonly NavigationItems = NavigationItems;
  displayedColumns: string[] = ['userId', 'username', 'email', 'roles'];
  dataSource: Observable<UserListRow[]> = of([]);
  resultsLength = 0;
  isLoadingResults = true;
  pageSize = 25;
  pageSizeOptions = [10, 25, 50];
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    this.dataSource =
      merge(this.sort.sortChange, this.paginator.page)
        .pipe(
          startWith({}),
          switchMap(() => {
            this.isLoadingResults = true;

            return this.userService.getUserList(
              this.sort.active,
              this.sort.direction,
              this.paginator.pageIndex,
              this.paginator.pageSize
            ).pipe(catchError(() => of(null)))
          }),
          map(data => {
            this.isLoadingResults = false;

            if (data === null) {
              return []
            }

            this.resultsLength = data.totalCount;
            return data.items;
          })
        );
  }
}
