import { Role } from "../../shared-module";

export class NavigationItem {
  routerUrlPattern: string;
  navigationUrl: string;
  roles: Role[] = []

  constructor(routerUrlPatter: string, navigationUrl: string, roles: Role[]) {
    this.routerUrlPattern = routerUrlPatter;
    this.navigationUrl = navigationUrl;
    this.roles = roles;
  }
}