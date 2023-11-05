import { NavbarItem } from "./navbar-item";
import { NavigationItems } from "./navigation-items";

export const NAVBAR_ITEMS: NavbarItem[] = [
  new NavbarItem('Home', NavigationItems.home, 'home'),
  new NavbarItem('Courses', NavigationItems.courseList, 'school'),
  new NavbarItem('Users', NavigationItems.userList, 'manage_accounts')
];