import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupAssignmentCreateDialogComponent } from './group-assignment-create-dialog.component';

describe('GroupAssignmentAddDialogComponent', () => {
  let component: GroupAssignmentCreateDialogComponent;
  let fixture: ComponentFixture<GroupAssignmentCreateDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupAssignmentCreateDialogComponent]
    });
    fixture = TestBed.createComponent(GroupAssignmentCreateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
