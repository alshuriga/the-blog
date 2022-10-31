import { TestBed } from '@angular/core/testing';

import { ServerValidationService } from './server-validation.service';

describe('ServerValidationService', () => {
  let service: ServerValidationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServerValidationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
