import { RoleModel } from "../role/role-model";

export class UserModel {
  userId!: string
  username!: string
  email!: string
  roles: RoleModel[] = []
}
