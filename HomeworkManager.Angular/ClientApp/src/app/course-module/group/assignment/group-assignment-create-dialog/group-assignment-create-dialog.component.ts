import { Component, inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { FormControl, FormGroup } from "@angular/forms";
import { AssignmentService } from "../../../../assignment-module";
import { GroupService } from "../../../services/group.service";
import { GroupInfo, NewAssignment } from "../../../../shared-module";

@Component({
  selector: 'hwm-group-assignment-create-dialog',
  templateUrl: './group-assignment-create-dialog.component.html',
  styleUrls: ['./group-assignment-create-dialog.component.scss']
})
export class GroupAssignmentCreateDialogComponent implements OnInit {
  private dialogRef = inject(MatDialogRef<GroupAssignmentCreateDialogComponent>);
  private assignmentService = inject(AssignmentService);
  private groupService = inject(GroupService);
  private groupName: string = inject(MAT_DIALOG_DATA);
  createAssignmentForm!: FormGroup;

  get name() {
    return this.createAssignmentForm.get('name')!!;
  }

  ngOnInit() {
    this.setupForm();
  }

  onOkClick() {
    const newAssignment = new NewAssignment()

    newAssignment.name = this.name.value;

    const groupInfo = new GroupInfo()

    groupInfo.courseId = this.groupService.courseId;
    groupInfo.name = this.groupName;
    newAssignment.groupInfo = groupInfo;

    this.assignmentService.createAssignment(newAssignment)
      .subscribe(assignmentId => {
        this.dialogRef.close(assignmentId);
      });
  }

  onCancelClick() {
    this.dialogRef.close();
  }

  private setupForm() {
    this.createAssignmentForm = new FormGroup({
      name: new FormControl('')
    });
  }
}
