import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { ControlModule } from "../control-module/control.module";
import { RouterOutlet } from "@angular/router";


@NgModule({
  declarations: [
    HomeComponent,
    DashboardComponent,
  ],
  imports: [
    CommonModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    ControlModule,
    RouterOutlet
  ],
  exports: [
    HomeComponent,
    DashboardComponent
  ]
})
export class HomeworkManagerModule {
}
