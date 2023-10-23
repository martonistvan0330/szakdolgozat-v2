import { RoleModel } from "./role-model";

export class UserModel {
  userId!: string
  username!: string
  email!: string
  roles: RoleModel[] = []
}
