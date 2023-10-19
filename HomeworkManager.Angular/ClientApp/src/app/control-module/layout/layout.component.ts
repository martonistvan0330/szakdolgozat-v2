import { Component, inject, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from "@angular/cdk/layout";
import { Observable } from "rxjs";
import { map, shareReplay } from "rxjs/operators";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";

@Component({
  selector: 'hwm-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {
  @ViewChild('nav_menu') navMenu!: NavMenuComponent;

  private breakpointObserver = inject(BreakpointObserver);
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  async toggleNavbar() {
    await this.navMenu.toggle();
  }
}
