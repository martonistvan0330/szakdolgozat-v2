import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavMenuComponent } from './layout/nav-menu/nav-menu.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { RouterLink, RouterOutlet } from "@angular/router";
import { LayoutComponent } from './layout/layout.component';
import { ToolbarComponent } from './layout/toolbar/toolbar.component';
import { MatMenuModule } from "@angular/material/menu";

@NgModule({
  declarations: [
    NavMenuComponent,
    LayoutComponent,
    ToolbarComponent
  ],
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatMenuModule,
    RouterOutlet,
    RouterLink
  ],
  exports: [
    NavMenuComponent,
    LayoutComponent,
    ToolbarComponent
  ]
})
export class ControlModule {
}
