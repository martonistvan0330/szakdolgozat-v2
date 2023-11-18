import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupAssignmentListComponent } from './group-assignment-list.component';

describe('AssignmentListComponent', () => {
  let component: GroupAssignmentListComponent;
  let fixture: ComponentFixture<GroupAssignmentListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupAssignmentListComponent]
    });
    fixture = TestBed.createComponent(GroupAssignmentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
