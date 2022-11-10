import {Component, OnInit} from '@angular/core';
import {WifiWizard2} from '@awesome-cordova-plugins/wifi-wizard-2/ngx';
import {Geolocation} from '@awesome-cordova-plugins/geolocation/ngx';
import {BehaviorSubject} from 'rxjs';
import {Meassurement} from '../model/meassurement.model';
import {Geoposition} from "@awesome-cordova-plugins/geolocation";
import {returnDownBack} from "ionicons/icons";

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss']
})
export class Tab1Page implements OnInit {

  scanResult: BehaviorSubject<Meassurement | string>;

  wifiScanResult: BehaviorSubject<any>;
  posResultHighAcc: BehaviorSubject<{latitude: number; longitude: number; altitude: number; accuracy: number} | {error: any}>;
  posResultLowAcc: BehaviorSubject<{latitude: number; longitude: number; altitude: number; accuracy: number} | {error: any}>;

  constructor(
    public wifiWizard: WifiWizard2,
    private geolocation: Geolocation,
  ) {
  }

  async ngOnInit(): Promise<any> {
    this.scanResult = new BehaviorSubject(null);

    this.wifiScanResult = new BehaviorSubject<any>({});
    this.posResultHighAcc = new BehaviorSubject(null);
    this.posResultLowAcc = new BehaviorSubject(null);

    await this.wifiWizard.requestPermission();
  }

  public async scan(): Promise<void> {
    let scanR: any;
    let posH: Geoposition;
    let posL: Geoposition;

    await this.wifiWizard.scan()
      .then(result => scanR = result)
      .catch(error => {
        this.scanResult.next(error);
        return Promise.reject(error);
      });
    await this.geolocation.getCurrentPosition( {enableHighAccuracy: false, maximumAge: 0})
      .then(result => posL = result)
      .catch(error => {return Promise.reject(error)});
    await new Promise(resolve => setTimeout(resolve, 1000));
    await this.geolocation.getCurrentPosition( {enableHighAccuracy: true, maximumAge: 0})
      .then(result => posH = result)
      .catch(error => {return Promise.reject(error)});

    this.scanResult.next(new Meassurement(scanR, posL, posH));

    // await this.scanNetworks();
    // await this.updatePos();
  }

  public async scanNetworks(): Promise<any> {
    await this.wifiWizard.scan().then(result => {
      this.wifiScanResult.next(result);
      console.log('WiFiScan:');
      console.log(result);
    }).catch(reason => this.wifiScanResult.next({reason}));
  }

  public async updatePos(): Promise<void> {
    await this.geolocation.getCurrentPosition( {enableHighAccuracy: false, maximumAge: 0}).then(result => {
      this.posResultLowAcc.next({
        latitude: result.coords.latitude,
        longitude: result.coords.latitude,
        altitude: result.coords.altitude,
        accuracy: result.coords.accuracy
      });
      console.log('PosLowAcc:');
      console.log(result);
    }).catch(error => {
      this.posResultLowAcc.next({error});
    });

    await new Promise(resolve => setTimeout(resolve, 1000));

    await this.geolocation.getCurrentPosition( {enableHighAccuracy: true, maximumAge: 0}).then(result => {
      this.posResultHighAcc.next({
        latitude: result.coords.latitude,
        longitude: result.coords.latitude,
        altitude: result.coords.altitude,
        accuracy: result.coords.accuracy
      });
      console.log('PosHighAcc:');
      console.log(result);
    }).catch(error => {
      this.posResultHighAcc.next({error});
    });
  }

}
