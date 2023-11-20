import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FileSubmissionDetailsComponent } from './file-submission-details.component';

describe('FileSubmissionDetailsComponent', () => {
  let component: FileSubmissionDetailsComponent;
  let fixture: ComponentFixture<FileSubmissionDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [FileSubmissionDetailsComponent]
    });
    fixture = TestBed.createComponent(FileSubmissionDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
