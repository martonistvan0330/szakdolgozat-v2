import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { NavigationItems, SnackBarService } from "../../../core-module";
import { GroupService } from "../../services/group.service";
import { GroupModel } from "../../../shared-module";
import { ActivatedRoute } from "@angular/router";
import { Subject } from "rxjs";

@Component({
  selector: 'hwm-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.scss']
})
export class GroupDetailsComponent implements OnInit, OnDestroy {
  private activatedRoute = inject(ActivatedRoute);
  private groupService = inject(GroupService);
  private snackBarService = inject(SnackBarService);
  private destroy$ = new Subject<void>();
  protected readonly NavigationItems = NavigationItems;
  group!: GroupModel;
  editUrl: string = `../../${NavigationItems.groupEdit.navigationUrl}/General`;

  ngOnInit() {
    this.activatedRoute.data
      .subscribe(({ group }) => {
        const groupModel = group as GroupModel;
        this.setUp(groupModel);
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private setUp(groupModel: GroupModel) {
    this.group = groupModel;

    if (groupModel) {
      this.editUrl = `../../${NavigationItems.groupEdit.navigationUrl}/${this.group.name}`
    } else {
      this.snackBarService.error('Something went wrong!');
    }
  }
}
