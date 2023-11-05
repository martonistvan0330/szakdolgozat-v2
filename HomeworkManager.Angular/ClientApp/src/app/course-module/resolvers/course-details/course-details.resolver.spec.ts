import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { courseDetailsResolver } from './course-details.resolver';

describe('courseDetailsResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => courseDetailsResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
