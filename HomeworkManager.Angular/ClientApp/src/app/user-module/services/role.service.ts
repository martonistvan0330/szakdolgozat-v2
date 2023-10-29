import { inject, Injectable } from '@angular/core';
import { RoleModel } from "../../shared-module";
import { AuthorizedApiClientService } from "../../services";

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  private authApiClient = inject(AuthorizedApiClientService)

  getRoles() {
    return this.authApiClient.get<RoleModel[]>('Role');
  }
}
