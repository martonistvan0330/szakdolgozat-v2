import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { GroupService } from "../../../services/group.service";
import { Observable } from "rxjs";
import { ColumnDefinition, UserListRow } from "../../../../shared-module";

@Component({
  selector: 'hwm-group-teacher-add-dialog',
  templateUrl: './group-teacher-add-dialog.component.html',
  styleUrls: ['./group-teacher-add-dialog.component.scss']
})
export class GroupTeacherAddDialogComponent implements OnInit {
  private groupService = inject(GroupService);
  private groupName: string = inject(MAT_DIALOG_DATA);
  protected readonly UserListRow = UserListRow;
  dataSource!: Observable<UserListRow[]>;
  columnDefs = [
    new ColumnDefinition('Full name', 'fullName'),
    new ColumnDefinition('Email', 'email')
  ];

  ngOnInit() {
    this.dataSource = this.groupService.getAddableTeachers(this.groupName);
  }
}
