import {Injectable} from '@angular/core';
import {MeasurementModel} from '../../model/measurement.model';
import {BehaviorSubject, Observable} from 'rxjs';
import {WifiWizard2} from '@awesome-cordova-plugins/wifi-wizard-2/ngx';
import {StorageService} from '../storage-service/storage.service';
import {ApiService} from '../api-service/api.service';
import {first} from 'rxjs/operators';
import {MeasurementBackendModel} from '../../model/measurement-backend.model';
import {PositionModel} from '../../model/position.model';
import {Geolocation, Position} from '@capacitor/geolocation';
import { Device } from '@capacitor/device';

@Injectable({
  providedIn: 'root'
})
export class ApScanService {

  public scanResult: BehaviorSubject<MeasurementModel>;

  public wifiScanResult: BehaviorSubject<any>;
  public posResultHighAcc: BehaviorSubject<{ latitude: number; longitude: number; altitude: number; accuracy: number } | { error: any }>;
  public posResultLowAcc: BehaviorSubject<{ latitude: number; longitude: number; altitude: number; accuracy: number } | { error: any }>;

  public storedMeasurements: MeasurementModel[];

  public latestFailed: boolean;

  public deviceName: string;

  public $scanning: BehaviorSubject<boolean>;

  constructor(
    public wifiWizard: WifiWizard2,
    private storageService: StorageService,
    private apiService: ApiService
  ) {
    this.scanResult = new BehaviorSubject(null);

    this.wifiScanResult = new BehaviorSubject<any>({});
    this.posResultHighAcc = new BehaviorSubject(null);
    this.posResultLowAcc = new BehaviorSubject(null);

    this.$scanning = new BehaviorSubject(false);

    this.storageService.get('measurements').then(m => {
      if (m) {
        this.storedMeasurements = m;
      } else {
        this.storedMeasurements = [];
      }
      console.log(this.storedMeasurements);
    });

    Device.getInfo().then( info => {
      this.deviceName = `${info.model} (${info.manufacturer})`;
      console.log(this.deviceName);
    });
  }

  public async scan(): Promise<void> {
    let scanR: any;
    let posH: Position;
    let posL: Position;

    console.log('Scan started');
    console.log('scanning WiFi-Networks...');

    this.$scanning.next(true);

    await this.wifiWizard.scan()
      .then(result => {scanR = result; console.log(result);})
      .catch(error => {
        this.scanResult.next(error);
        console.log(error);
        return Promise.reject(error);
      });

    console.log('getting Position (Low Accuracy)...');
    await Geolocation.getCurrentPosition({enableHighAccuracy: false, timeout: 5000, maximumAge: 0})
      .then(result => {posL = result; console.log(result);})
      .catch(error => {console.log(error); Promise.reject(error);});

    await new Promise(resolve => setTimeout(resolve, 1000));

    console.log('getting Position (High Accuracy)...');
    await Geolocation.getCurrentPosition({enableHighAccuracy: true, timeout: 5000, maximumAge: 0})
      .then(result => {posH = result; console.log(result);})
      .catch(error => {console.log(error); Promise.reject(error);});

    const m = new MeasurementModel(scanR, posL, posH);
    console.log('Result:');
    console.log(m);
    this.scanResult.next(m);
    this.saveMeasurement(m);
    this.$scanning.next(false);
  }

  public async scanNetworks(): Promise<any> {
    this.$scanning.next(true);
    await this.wifiWizard.scan().then(result => {
      this.wifiScanResult.next(result);
      console.log('WiFiScan:');
      console.log(result);
      this.$scanning.next(false);
    }).catch(reason => {
      this.wifiScanResult.next({reason});
      this.$scanning.next(false);
    });
  }

  public async updatePos(): Promise<void> {
    await Geolocation.getCurrentPosition({enableHighAccuracy: false, maximumAge: 0}).then(result => {
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

    await Geolocation.getCurrentPosition({enableHighAccuracy: true, maximumAge: 0}).then(result => {
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

  public send(realPos: PositionModel): void {
    const scanResult = this.scanResult.getValue();
    if (scanResult instanceof MeasurementModel) {
      const scanBackend = new MeasurementBackendModel(scanResult, realPos, this.deviceName);
      console.log(scanBackend);
      console.log(this.deviceName);
      this.apiService.writeMeasurement(scanBackend).pipe(first()).subscribe();
    }
  }

  private saveMeasurement(m: MeasurementModel): void {
    const newStoredMeasurements: MeasurementModel[] = [m];

    for (let i = 0; i < 9; i++) {
      if (this.storedMeasurements[i]) {
        newStoredMeasurements[i + 1] = this.storedMeasurements[i];
      }
    }

    this.storageService.set('measurements', newStoredMeasurements);
    this.storedMeasurements = newStoredMeasurements;
    console.log(this.storedMeasurements);
  }

}
