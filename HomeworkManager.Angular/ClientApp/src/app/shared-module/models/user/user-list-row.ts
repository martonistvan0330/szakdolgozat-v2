export class UserListRow {
  userId!: string
  fullName!: string
  username!: string
  email!: string
  roles!: string

  static compareFullName(user1: UserListRow, user2: UserListRow) {
    if (user1.fullName < user2.fullName) {
      return -1;
    } else if (user1.fullName == user2.fullName) {
      return 0;
    } else {
      return 1
    }
  }

  static displayFn(user: UserListRow): string {
    return user
      ? user.fullName + '(' + user.email + ')'
      : '';
  }
}