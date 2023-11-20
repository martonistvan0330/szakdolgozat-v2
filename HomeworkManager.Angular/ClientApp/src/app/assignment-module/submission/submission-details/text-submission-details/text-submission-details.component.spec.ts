import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextSubmissionDetailsComponent } from './text-submission-details.component';

describe('TextSubmissionDetailsComponent', () => {
  let component: TextSubmissionDetailsComponent;
  let fixture: ComponentFixture<TextSubmissionDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TextSubmissionDetailsComponent]
    });
    fixture = TestBed.createComponent(TextSubmissionDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
