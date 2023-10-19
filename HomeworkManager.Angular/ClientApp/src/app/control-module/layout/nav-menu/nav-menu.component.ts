import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { MatSidenav } from "@angular/material/sidenav";

@Component({
  selector: 'hwm-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {
  @Input() isMobile: boolean | null = false;
  @Output() sidenavRef = new EventEmitter<MatSidenav>;
  @ViewChild('sidenav') sidenav!: MatSidenav;

  async toggle() {
    await this.sidenav.toggle();
  }
}
