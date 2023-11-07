import { Component, inject, OnInit } from '@angular/core';
import { NavigationItems, SnackBarService } from "../../../core-module";
import { GroupService } from "../../services/group.service";
import { GroupModel } from "../../../shared-module";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'hwm-group-details',
  templateUrl: './group-details.component.html',
  styleUrls: ['./group-details.component.scss']
})
export class GroupDetailsComponent implements OnInit {
  private activatedRoute = inject(ActivatedRoute);
  private groupService = inject(GroupService);
  private snackBarService = inject(SnackBarService);
  protected readonly NavigationItems = NavigationItems;
  group: GroupModel | null = null;
  isLoadingResults = true;
  editUrl!: string;

  ngOnInit() {
    this.editUrl = `../../${NavigationItems.groupEdit.navigationUrl}/General`

    this.activatedRoute.data
      .subscribe(({ group }) => {
        const groupModel = group as GroupModel;
        this.group = groupModel;

        if (groupModel) {
          this.editUrl = `../../${NavigationItems.groupEdit.navigationUrl}/${group.name}`
        } else {
          this.snackBarService.error('Something went wrong!');
        }
      });
  }
}
