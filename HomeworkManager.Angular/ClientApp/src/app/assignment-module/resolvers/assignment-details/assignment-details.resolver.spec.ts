import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { assignmentDetailsResolver } from './assignment-details.resolver';

describe('assignmentDetailsResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => assignmentDetailsResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
