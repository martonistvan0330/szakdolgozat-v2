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

  public static emailConfirmation: NavigationItem = {
    routerUrlPattern: 'email-confirmation',
    navigationUrl: '/email-confirmation',
    roles: []
  }

  public static passwordRecovery: NavigationItem = {
    routerUrlPattern: 'password-recovery',
    navigationUrl: '/password-recovery',
    roles: []
  }

  public static passwordReset: NavigationItem = {
    routerUrlPattern: 'password-reset',
    navigationUrl: '/password-reset',
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