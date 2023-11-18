import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavMenuComponent } from './layout/nav-menu/nav-menu.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { RouterLink, RouterLinkActive, RouterOutlet } from "@angular/router";
import { LayoutComponent } from './layout/layout.component';
import { ToolbarComponent } from './layout/toolbar/toolbar.component';
import { MatMenuModule } from "@angular/material/menu";
import { TableComponent } from './table/table.component';
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatOptionModule, MatRippleModule } from "@angular/material/core";
import { MatSortModule } from "@angular/material/sort";
import { MatTableModule } from "@angular/material/table";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { ReactiveFormsModule } from "@angular/forms";
import { ListAddDialogComponent } from './list-add-dialog/list-add-dialog.component';
import { MatAutocompleteModule } from "@angular/material/autocomplete";
import { MatDialogModule } from "@angular/material/dialog";
import { MatCheckboxModule } from "@angular/material/checkbox";

@NgModule({
  declarations: [
    NavMenuComponent,
    LayoutComponent,
    ToolbarComponent,
    TableComponent,
    ListAddDialogComponent
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
    RouterLink,
    RouterLinkActive,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatRippleModule,
    MatSortModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatAutocompleteModule,
    MatDialogModule,
    MatOptionModule,
    MatCheckboxModule
  ],
  exports: [
    NavMenuComponent,
    LayoutComponent,
    ToolbarComponent,
    TableComponent,
    ListAddDialogComponent
  ]
})
export class ControlModule {
}
