import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseTeacherAddDialogComponent } from './course-teacher-add-dialog.component';

describe('CourseTeacherAddDialogComponent', () => {
  let component: CourseTeacherAddDialogComponent;
  let fixture: ComponentFixture<CourseTeacherAddDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CourseTeacherAddDialogComponent]
    });
    fixture = TestBed.createComponent(CourseTeacherAddDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
