import {Geoposition} from '@awesome-cordova-plugins/geolocation';
import {ApScanItemModel} from './ap-scan-item.model';
import {PositionModel} from './position.model';


export class MeasurementModel {

  public measurements: ApScanItemModel[];

  public positionLowAccuracy: PositionModel;

  public positionHighAccuracy: PositionModel;

  public timestamp: Date;

  public device: string;

  constructor(
    measurements: any[],
    positionLowAccuracy?: Geoposition,
    positionHighAccuracy?: Geoposition,
  ) {
    this.measurements = [];
    for (let i = 0; i < measurements.length; i++) {
      this.measurements[i] = {
        SSID: measurements[i].SSID,
        MAC: measurements[i].BSSID,
        level: measurements[i].level,
      };
    }
    this.measurements = this.measurements.filter(m => m.SSID.includes('DHBW-KA')).sort((m1, m2) =>
      m2.level - m1.level);

    if (positionHighAccuracy) {
      this.positionHighAccuracy = new PositionModel(
        positionHighAccuracy.coords.latitude,
        positionHighAccuracy.coords.longitude,
        positionHighAccuracy.coords.altitude,
        positionHighAccuracy.coords.accuracy
      );
    }

    if (positionLowAccuracy) {
      this.positionLowAccuracy = new PositionModel(
        positionLowAccuracy.coords.latitude,
        positionLowAccuracy.coords.longitude,
        positionLowAccuracy.coords.altitude,
        positionLowAccuracy.coords.accuracy
      );
    }

    this.timestamp = new Date(Date.now());
    this.device = "Honor";
  }
}
