import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoSnackBarComponent } from './info-snack-bar.component';

describe('InfoSnackBarComponent', () => {
  let component: InfoSnackBarComponent;
  let fixture: ComponentFixture<InfoSnackBarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InfoSnackBarComponent]
    });
    fixture = TestBed.createComponent(InfoSnackBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
