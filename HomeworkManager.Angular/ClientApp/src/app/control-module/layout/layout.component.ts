import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from "@angular/cdk/layout";
import { Observable } from "rxjs";
import { map, shareReplay } from "rxjs/operators";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { AuthService, NAVBAR_ITEMS, NavbarItem } from "../../core-module";
import { UserModel } from "../../shared-module";

@Component({
  selector: 'hwm-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  private breakpointObserver = inject(BreakpointObserver);
  private authService = inject(AuthService);
  @ViewChild('nav_menu') navMenu!: NavMenuComponent;
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  visibleNavbarItems: NavbarItem[] = []

  async toggleNavbar() {
    await this.navMenu.toggle();
  }

  ngOnInit() {
    this.authService.currentUser$
      .subscribe(user => {
        if (user) {
          this.visibleNavbarItems = this.getVisibleNavbarItems(user);
        } else {
          this.visibleNavbarItems = [];
        }
      }).unsubscribe();
  }

  private getVisibleNavbarItems(user: UserModel) {
    return NAVBAR_ITEMS.filter(navbarItem => {
      if (!navbarItem.navigationItem.roles || navbarItem.navigationItem.roles.length <= 0) {
        return true;
      } else {
        return this.authService.userHasRole(user, navbarItem.navigationItem.roles);
      }
    });
  }
}
