import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { Role, RoleModel, UserModel } from "../../shared-module";
import { RoleService } from "../services/role.service";
import { FormControl } from "@angular/forms";
import { UserService } from "../services/user.service";
import { AuthService } from "../../core-module";

@Component({
  selector: 'hwm-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.scss']
})
export class UserDetailComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private userService = inject(UserService);
  private roleService = inject(RoleService);
  saveClicked = false;
  roles: FormControl<number[] | null> = new FormControl({ value: null, disabled: true });
  allRoles: RoleModel[] = [];
  user: UserModel | null = null;
  isAdministrator = false;

  ngOnInit() {
    this.roleService.getRoles()
      .subscribe(roles => {
        this.allRoles = roles;
      });

    this.activatedRoute.data
      .subscribe(({ user }) => {
        const userModel = user as UserModel;
        this.user = userModel;
        if (userModel) {
          const roleIds = userModel.roles.map(role => role.roleId);

          this.roles.setValue(roleIds);
        }
      });

    this.authService.hasRole([Role.ADMINISTRATOR])
      .subscribe(isAdmin => {
        this.isAdministrator = isAdmin;

        if (isAdmin) {
          this.roles.enable();
        } else {
          this.roles.disable();
        }
      });
  }

  updateRoles() {
    this.saveClicked = true;

    if (this.user && this.user.userId && this.roles.value && this.roles.value.length > 0) {
      this.userService.updateRoles(this.user.userId, this.roles.value!);
    }
  }
}
