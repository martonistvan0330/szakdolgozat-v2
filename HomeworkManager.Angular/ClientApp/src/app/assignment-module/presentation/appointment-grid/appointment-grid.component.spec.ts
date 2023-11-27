import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointmentGridComponent } from './appointment-grid.component';

describe('AppointmentGridComponent', () => {
  let component: AppointmentGridComponent;
  let fixture: ComponentFixture<AppointmentGridComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AppointmentGridComponent]
    });
    fixture = TestBed.createComponent(AppointmentGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
