import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmentDetailDialogComponent } from './appointment-detail-dialog.component';

describe('AppointmentDetailDialogComponent', () => {
  let component: AppointmentDetailDialogComponent;
  let fixture: ComponentFixture<AppointmentDetailDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AppointmentDetailDialogComponent]
    });
    fixture = TestBed.createComponent(AppointmentDetailDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
