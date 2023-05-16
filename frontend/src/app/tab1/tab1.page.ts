import {Component} from '@angular/core';
import {ApScanService} from '../services/ap-scan-service/ap-scan.service';

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss']
})
export class Tab1Page {

  constructor(
    public apScanService: ApScanService
  ) {
  }

}
