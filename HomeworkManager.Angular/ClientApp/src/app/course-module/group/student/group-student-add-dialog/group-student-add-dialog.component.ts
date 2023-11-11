import { Component, inject, OnInit } from '@angular/core';
import { GroupService } from "../../../services/group.service";
import { MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Observable } from "rxjs";
import { ColumnDefinition, UserListRow } from "../../../../shared-module";

@Component({
  selector: 'hwm-group-student-add-dialog',
  templateUrl: './group-student-add-dialog.component.html',
  styleUrls: ['./group-student-add-dialog.component.scss']
})
export class GroupStudentAddDialogComponent implements OnInit {
  private groupService = inject(GroupService);
  private groupName: string = inject(MAT_DIALOG_DATA);
  protected readonly UserListRow = UserListRow;
  dataSource!: Observable<UserListRow[]>;
  columnDefs = [
    new ColumnDefinition('Full name', 'fullName'),
    new ColumnDefinition('Email', 'email')
  ];

  ngOnInit() {
    this.dataSource = this.groupService.getAddableStudents(this.groupName);
  }
}
