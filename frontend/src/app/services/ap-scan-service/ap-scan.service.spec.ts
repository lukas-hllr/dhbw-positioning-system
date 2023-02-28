import { TestBed } from '@angular/core/testing';

import { ApScanService } from './ap-scan.service';

describe('ApScanServiceService', () => {
  let service: ApScanService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApScanService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
