import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseStudentAddDialogComponent } from './course-student-add-dialog.component';

describe('CourseStudentAddDialogComponent', () => {
  let component: CourseStudentAddDialogComponent;
  let fixture: ComponentFixture<CourseStudentAddDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CourseStudentAddDialogComponent]
    });
    fixture = TestBed.createComponent(CourseStudentAddDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
