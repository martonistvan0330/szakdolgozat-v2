import { NavigationItem } from "./navigation-item";
import { Role } from "../../shared-module";

export class NavigationItems {
  public static login: NavigationItem = {
    routerUrlPattern: 'login',
    navigationUrl: '/login',
    roles: []
  }

  public static register: NavigationItem = {
    routerUrlPattern: 'register',
    navigationUrl: '/register',
    roles: []
  }

  public static home: NavigationItem = {
    routerUrlPattern: 'home',
    navigationUrl: '/home',
    roles: []
  }

  public static userDetail: NavigationItem = {
    routerUrlPattern: 'users/:id',
    navigationUrl: '/users',
    roles: []
  }

  public static userList: NavigationItem = {
    routerUrlPattern: 'users',
    navigationUrl: '/users',
    roles: [Role.ADMINISTRATOR]
  }
}