import { Component, inject } from '@angular/core';
import { Errors, GroupModel, UpdateGroup } from "../../../shared-module";
import { ActivatedRoute, Router } from "@angular/router";
import { NavigationItems, SnackBarService } from "../../../core-module";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { GroupService } from "../../services/group.service";
import {
  uniqueGroupNameAsyncValidator
} from "../../validators/unique-group-name/unique-group-name-async-validator.directive";

@Component({
  selector: 'hwm-group-edit',
  templateUrl: './group-edit.component.html',
  styleUrls: ['./group-edit.component.scss']
})
export class GroupEditComponent {
  private activatedRoute = inject(ActivatedRoute);
  private groupService = inject(GroupService);
  private router = inject(Router);
  private snackBarService = inject(SnackBarService);
  protected readonly Errors = Errors;
  group: GroupModel | null = null;
  groupEditForm!: FormGroup;
  isLoading = true;
  nameError = Errors.NoError;

  get name() {
    return this.groupEditForm.get('name')!!;
  }

  get description() {
    return this.groupEditForm.get('description')!!;
  }

  ngOnInit() {
    this.activatedRoute.data
      .subscribe(({ group }) => {
        const groupModel = group as GroupModel;
        this.group = groupModel;

        if (groupModel) {
          this.setup()
        } else {
          this.snackBarService.error('Something went wrong!');
        }
      });
  }

  update() {
    if (this.groupEditForm.invalid) {
      return;
    }

    this.isLoading = true;

    const updatedGroup = new UpdateGroup();
    updatedGroup.name = this.name.value;
    updatedGroup.description = this.description.value ? this.description.value : null;

    this.groupService.updateGroup(this.group!!.name, updatedGroup)
      .subscribe({
        next: success => {
          this.router.navigate(
            [`../../../${NavigationItems.groupDetails.navigationUrl}/${updatedGroup.name}`],
            { relativeTo: this.activatedRoute }
          ).then(success => {
            if (success) {
              this.snackBarService.success('Group updated');
            }
          });
        },
        error: error => {
          this.snackBarService.error('Group update failed', error.error);
          this.isLoading = false;
        }
      });
  }

  private setup() {
    this.formSetup();

    this.nameControlSetup();

    this.isLoading = false;
  }

  private formSetup() {
    this.groupEditForm = new FormGroup({
      name: new FormControl(this.group!!.name, {
        validators: [Validators.required,],
        asyncValidators: [uniqueGroupNameAsyncValidator(this.groupService, this.group?.name)],
        updateOn: 'change'
      }),
      description: new FormControl(this.group!!.description),
    });
  }

  private nameControlSetup() {
    this.name.statusChanges
      .subscribe(() => {
        if (this.name.status === 'VALID') {
          this.nameError = Errors.NoError;
        }

        if (this.name.status === 'INVALID') {
          if (this.name.errors?.['required']) {
            this.nameError = Errors.Required;
          } else if (this.name.errors?.['uniqueName']) {
            this.nameError = Errors.Unique;
          }
        }
      });
  }
}
