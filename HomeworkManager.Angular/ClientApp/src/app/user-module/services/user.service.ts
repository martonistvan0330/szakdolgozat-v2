import { inject, Injectable } from '@angular/core';
import { Pageable, PageableOptions, UserListRow, UserModel } from "../../shared-module";
import { AuthorizedApiClientService } from "../../services";
import { delay } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private authApiClient = inject(AuthorizedApiClientService);

  getUser(userId: string) {
    return this.authApiClient.get<UserModel | null>('User/' + userId);
  }

  getUserList(options: PageableOptions | null = null) {
    let requestUrl = 'User'

    if (options != null) {
      requestUrl = 'User'
        + '?pageData.pageIndex=' + options.pageData?.pageIndex
        + '&pageData.pageSize=' + options.pageData?.pageSize
        + '&sortOptions.sort=' + options.sortOptions?.sort
        + '&sortOptions.sortDirection=' + options.sortOptions?.sortDirection
        + '&searchText=' + options.searchText;
    }

    return this.authApiClient.get<Pageable<UserListRow>>(requestUrl).pipe(delay(1000));
  }

  updateRoles(userId: string, roles: number[]) {
    return this.authApiClient.put<boolean>('User/' + userId + '/Roles', roles);
  }
}
