import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QweComponent } from './qwe.component';

describe('QweComponent', () => {
  let component: QweComponent;
  let fixture: ComponentFixture<QweComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [QweComponent]
    });
    fixture = TestBed.createComponent(QweComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
