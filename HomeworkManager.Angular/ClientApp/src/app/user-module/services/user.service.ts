import { inject, Injectable } from '@angular/core';
import { SortDirection } from "@angular/material/sort";
import { Pageable, UserListRow, UserModel } from "../../shared-module";
import { delay } from "rxjs";
import { AuthorizedApiClientService } from "../../services";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private authApiClient = inject(AuthorizedApiClientService);

  getUser(userId: string) {
    return this.authApiClient.get<UserModel>('User/' + userId);
  }

  getUserList(sort: string, sortDirection: SortDirection, page: number, pageSize: number) {
    const requestUrl = `User?sort=${sort}&sortDirection=${sortDirection}&page=${page}&pageSize=${pageSize}`;
    return this.authApiClient.get<Pageable<UserListRow>>(requestUrl).pipe(delay(1000));
  }

  updateRoles(userId: string, roles: number[]) {
    return this.authApiClient.put<boolean>('User/' + userId + '/Roles', roles);
  }
}
