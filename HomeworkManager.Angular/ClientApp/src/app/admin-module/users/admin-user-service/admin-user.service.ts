import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../../core-module";
import { Pageable, UserListRow } from "../../../shared-module";
import { SortDirection } from "@angular/material/sort";
import { delay } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AdminUserService {
  private authApiClient = inject(AuthorizedApiClientService)

  getUserList(sort: string, sortDirection: SortDirection, page: number, pageSize: number) {
    const requestUrl = `User?sort=${sort}&sortDirection=${sortDirection}&page=${page}&pageSize=${pageSize}`;
    return this.authApiClient.get<Pageable<UserListRow>>(requestUrl).pipe(delay(1000));
  }
}
