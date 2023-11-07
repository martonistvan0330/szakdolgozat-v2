import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { NavigationItems, SnackBarService } from "../../../core-module";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { GroupService } from "../../services/group.service";
import { Errors, NewGroup } from "../../../shared-module";
import {
  uniqueGroupNameAsyncValidator
} from "../../validators/unique-group-name/unique-group-name-async-validator.directive";

@Component({
  selector: 'hwm-group-create',
  templateUrl: './group-create.component.html',
  styleUrls: ['./group-create.component.scss']
})
export class GroupCreateComponent {
  private groupService = inject(GroupService);
  private router = inject(Router);
  private activatedRoute = inject(ActivatedRoute);
  private snackBarService = inject(SnackBarService);
  protected readonly Errors = Errors;
  groupCreateForm!: FormGroup;
  isLoading = false;
  nameError = Errors.NoError;

  get name() {
    return this.groupCreateForm.get('name')!!;
  }

  get description() {
    return this.groupCreateForm.get('description')!!;
  }

  ngOnInit() {
    this.formSetup();

    this.nameControlSetup();
  }

  create() {
    if (this.groupCreateForm.invalid) {
      return;
    }

    this.isLoading = true;

    const newGroup = new NewGroup();
    newGroup.name = this.name.value;
    newGroup.description = this.description.value ? this.description.value : null;

    this.groupService.createGroup(newGroup)
      .subscribe({
        next: groupId => {
          this.router.navigate([`../../${NavigationItems.groupDetails.navigationUrl}/${newGroup.name}`], { relativeTo: this.activatedRoute })
            .then(success => {
              if (success) {
                this.snackBarService.success('Group created');
              }
            });
        },
        error: error => {
          this.snackBarService.error('Group creation failed', error.error);
          this.isLoading = false;
        }
      });
  }

  private formSetup() {
    this.groupCreateForm = new FormGroup({
      name: new FormControl('', {
        validators: [Validators.required,],
        asyncValidators: [uniqueGroupNameAsyncValidator(this.groupService)],
        updateOn: 'change'
      }),
      description: new FormControl(''),
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
