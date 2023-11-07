import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { groupDetailsResolver } from './group-details.resolver';

describe('groupDetailsResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => groupDetailsResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
