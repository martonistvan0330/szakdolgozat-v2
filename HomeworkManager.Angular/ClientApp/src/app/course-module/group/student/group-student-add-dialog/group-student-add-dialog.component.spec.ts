import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupStudentAddDialogComponent } from './group-student-add-dialog.component';

describe('GroupStudentAddDialogComponent', () => {
  let component: GroupStudentAddDialogComponent;
  let fixture: ComponentFixture<GroupStudentAddDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupStudentAddDialogComponent]
    });
    fixture = TestBed.createComponent(GroupStudentAddDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
