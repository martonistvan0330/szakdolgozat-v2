import { TestBed } from '@angular/core/testing';

import { AuthorizedApiClientService } from './authorized-api-client.service';

describe('AuthorizedApiClientService', () => {
  let service: AuthorizedApiClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthorizedApiClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
