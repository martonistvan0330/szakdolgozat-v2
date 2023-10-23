import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserListComponent } from './users/user-list/user-list.component';
import { MatTableModule } from "@angular/material/table";
import { MatRippleModule } from "@angular/material/core";
import { MatSortModule } from "@angular/material/sort";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatDividerModule } from "@angular/material/divider";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";


@NgModule({
  declarations: [
    UserListComponent
  ],
  imports: [
    CommonModule,
    MatTableModule,
    MatRippleModule,
    MatSortModule,
    MatPaginatorModule,
    MatDividerModule,
    MatProgressSpinnerModule
  ],
  exports: [
    UserListComponent
  ]
})
export class AdminModule {
}
