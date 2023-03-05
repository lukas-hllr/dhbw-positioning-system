import {Injectable} from '@angular/core';
import {MeassurementModel} from '../../model/meassurement.model';
import {BehaviorSubject, Observable} from 'rxjs';
import {WifiWizard2} from '@awesome-cordova-plugins/wifi-wizard-2/ngx';
import {Geolocation} from '@awesome-cordova-plugins/geolocation/ngx';
import {StorageService} from '../storage-service/storage.service';
import {Geoposition} from '@awesome-cordova-plugins/geolocation';
import {ApiService} from "../api-service/api.service";

@Injectable({
  providedIn: 'root'
})
export class ApScanService {

  public scanResult: BehaviorSubject<MeassurementModel>;

  public wifiScanResult: BehaviorSubject<any>;
  public posResultHighAcc: BehaviorSubject<{ latitude: number; longitude: number; altitude: number; accuracy: number } | { error: any }>;
  public posResultLowAcc: BehaviorSubject<{ latitude: number; longitude: number; altitude: number; accuracy: number } | { error: any }>;

  public storedMeassurements: MeassurementModel[];

  public latestFailed: boolean;

  constructor(
    public wifiWizard: WifiWizard2,
    private geolocation: Geolocation,
    private storageService: StorageService,
    private apiService: ApiService
  ) {
    this.scanResult = new BehaviorSubject(null);

    this.wifiScanResult = new BehaviorSubject<any>({});
    this.posResultHighAcc = new BehaviorSubject(null);
    this.posResultLowAcc = new BehaviorSubject(null);

    this.storageService.get('meassurements').then(m => {
      if (m) {
        this.storedMeassurements = m;
      } else {
        this.storedMeassurements = [];
      }
    });
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
    await this.geolocation.getCurrentPosition({enableHighAccuracy: false, maximumAge: 0})
      .then(result => posL = result)
      .catch(error => Promise.reject(error));
    await new Promise(resolve => setTimeout(resolve, 1000));
    await this.geolocation.getCurrentPosition({enableHighAccuracy: true, maximumAge: 0})
      .then(result => posH = result)
      .catch(error => Promise.reject(error));

    const m = new MeassurementModel(scanR, posL, posH);
    this.scanResult.next(m);
    this.saveMeassurement(m);
  }

  public async scanNetworks(): Promise<any> {
    await this.wifiWizard.scan().then(result => {
      this.wifiScanResult.next(result);
      console.log('WiFiScan:');
      console.log(result);
    }).catch(reason => this.wifiScanResult.next({reason}));
  }

  public async updatePos(): Promise<void> {
    await this.geolocation.getCurrentPosition({enableHighAccuracy: false, maximumAge: 0}).then(result => {
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

    await this.geolocation.getCurrentPosition({enableHighAccuracy: true, maximumAge: 0}).then(result => {
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

  public send(): void {
    if (this.scanResult.getValue() instanceof MeassurementModel) {
      this.apiService.writeMeassurement(this.scanResult.getValue() as MeassurementModel);
    }
  }

  private saveMeassurement(m: MeassurementModel): void {
    const newStoredMeassurements: MeassurementModel[] = [m];

    for (let i = 0; i < 9; i++) {
      if (this.storedMeassurements[i]) {
        newStoredMeassurements[i + 1] = this.storedMeassurements[i];
      }
    }

    this.storageService.set('meassurements', newStoredMeassurements);
    this.storedMeassurements = newStoredMeassurements;
    console.log(this.storedMeassurements);
  }

}
