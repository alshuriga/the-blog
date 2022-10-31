import { TestBed } from '@angular/core/testing';

import { ServerErrorsInterceptor } from './server-errors.interceptor';

describe('ServerErrorsInterceptor', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      ServerErrorsInterceptor
      ]
  }));

  it('should be created', () => {
    const interceptor: ServerErrorsInterceptor = TestBed.inject(ServerErrorsInterceptor);
    expect(interceptor).toBeTruthy();
  });
});
