import { NavigationItem } from "./navigation-item";

export class NavbarItem {
  title: string;
  navigationItem: NavigationItem;
  icon: string;

  constructor(title: string, navigationItem: NavigationItem, icon: string = '') {
    this.title = title;
    this.navigationItem = navigationItem;
    this.icon = icon;
  }
}