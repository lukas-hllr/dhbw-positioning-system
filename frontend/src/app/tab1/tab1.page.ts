import {Component, OnInit} from '@angular/core';
import {WifiWizard2} from '@awesome-cordova-plugins/wifi-wizard-2/ngx';
import {Geolocation} from '@awesome-cordova-plugins/geolocation/ngx';
import {BehaviorSubject} from 'rxjs';
import {MeassurementModel} from '../model/meassurement.model';
import {Geoposition} from '@awesome-cordova-plugins/geolocation';
import {StorageService} from '../services/storage-service/storage.service';
import {ApiService} from '../services/api-service/api.service';
import {ApScanService} from "../services/ap-scan-service/ap-scan.service";

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
