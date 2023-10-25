import { inject, Injectable } from '@angular/core';
import { AuthorizedApiClientService } from "../../core-module";
import { RoleModel } from "../../shared-module";

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private authApiClient = inject(AuthorizedApiClientService)

  getRoles() {
    return this.authApiClient.get<RoleModel[]>('Role');
  }
}
