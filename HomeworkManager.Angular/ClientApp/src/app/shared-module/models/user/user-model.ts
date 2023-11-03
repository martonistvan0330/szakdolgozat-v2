import { RoleModel } from "../role/role-model";

export class UserModel {
  userId!: string
  fullName!: string
  username!: string
  email!: string
  emailConfirmed: boolean = false
  roles: RoleModel[] = []
}
