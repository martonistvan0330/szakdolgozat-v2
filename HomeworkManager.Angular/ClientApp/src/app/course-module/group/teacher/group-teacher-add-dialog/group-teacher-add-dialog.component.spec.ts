import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupTeacherAddDialogComponent } from './group-teacher-add-dialog.component';

describe('TeacherAddDialogComponent', () => {
  let component: GroupTeacherAddDialogComponent;
  let fixture: ComponentFixture<GroupTeacherAddDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [GroupTeacherAddDialogComponent]
    });
    fixture = TestBed.createComponent(GroupTeacherAddDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
