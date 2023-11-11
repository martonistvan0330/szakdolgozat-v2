import { Component, inject, Input, OnInit, ViewChild } from '@angular/core';
import { NavigationItems } from "../../../core-module";
import { GroupListRow } from "../../../shared-module";
import { MatSidenav } from "@angular/material/sidenav";
import { merge, startWith, switchMap } from "rxjs";
import { GroupService } from "../../services/group.service";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  selector: 'hwm-group-list',
  templateUrl: './group-list.component.html',
  styleUrls: ['./group-list.component.scss']
})
export class GroupListComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private router = inject(Router);
  private groupService = inject(GroupService);
  protected readonly NavigationItems = NavigationItems;
  @Input() courseId!: number
  @Input() isMobile: boolean | null = false;
  @ViewChild('sidenav') sidenav!: MatSidenav;
  groups: GroupListRow[] = [];

  ngOnInit() {
    merge(this.groupService.groupAdded$, this.groupService.groupUpdated$)
      .pipe(
        startWith({}),
        switchMap(() => {
          return this.groupService.getGroups()
        })
      )
      .subscribe(groups => {
        this.groups = groups;
      })
  }

  onGroupClick(groupName: string) {
    this.groupService.groupChange(groupName);
  }

  async toggle() {
    await this.sidenav.toggle();
  }
}
