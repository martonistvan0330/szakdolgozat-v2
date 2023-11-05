import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NavigationItems } from "../../../core-module";
import { GroupModel } from "../../../shared-module";
import { MatSidenav } from "@angular/material/sidenav";

@Component({
  selector: 'hwm-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.scss']
})
export class GroupListComponent implements OnInit {
  protected readonly NavigationItems = NavigationItems;
  @Input() courseId!: number
  @Input() groups: GroupModel[] = []
  @Input() isMobile: boolean | null = false;
  @ViewChild('sidenav') sidenav!: MatSidenav;
  courseUrl = ''

  ngOnInit() {
    this.courseUrl = NavigationItems.courseDetails.navigationUrl + '/' + this.courseId;
  }

  async toggle() {
    await this.sidenav.toggle();
  }
}
