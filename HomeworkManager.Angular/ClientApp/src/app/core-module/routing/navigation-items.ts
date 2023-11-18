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
    routerUrlPattern: 'users/:userId',
    navigationUrl: '/users',
    roles: []
  }

  public static userList: NavigationItem = {
    routerUrlPattern: 'users',
    navigationUrl: '/users',
    roles: [Role.ADMINISTRATOR]
  }

  public static courseList: NavigationItem = {
    routerUrlPattern: 'courses',
    navigationUrl: '/courses',
    roles: []
  }

  public static courseDetails: NavigationItem = {
    routerUrlPattern: 'courses/:courseId',
    navigationUrl: '/courses',
    roles: []
  }

  public static courseCreate: NavigationItem = {
    routerUrlPattern: 'courses/create',
    navigationUrl: '/courses/create',
    roles: [Role.ADMINISTRATOR, Role.TEACHER]
  }

  public static courseEdit: NavigationItem = {
    routerUrlPattern: 'courses/edit/:courseId',
    navigationUrl: '/courses/edit',
    roles: [Role.ADMINISTRATOR, Role.TEACHER]
  }

  public static groupDetails: NavigationItem = {
    routerUrlPattern: 'groups/:groupName',
    navigationUrl: 'groups',
    roles: []
  }

  public static groupCreate: NavigationItem = {
    routerUrlPattern: 'groups/create',
    navigationUrl: 'groups/create',
    roles: [Role.ADMINISTRATOR, Role.TEACHER]
  }

  public static groupEdit: NavigationItem = {
    routerUrlPattern: 'groups/edit/:groupName',
    navigationUrl: 'groups/edit',
    roles: [Role.ADMINISTRATOR, Role.TEACHER]
  }

  public static assignmentDetails: NavigationItem = {
    routerUrlPattern: 'assignments/:assignmentId',
    navigationUrl: '/assignments',
    roles: []
  }
}