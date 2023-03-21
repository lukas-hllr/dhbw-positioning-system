import {MeasurementEntity} from './measurement-entity';
import {PositionModel} from './position.model';
import {Position} from '@capacitor/geolocation';


export class MeasurementModel {

  public measurements: MeasurementEntity[];

  public positionHighAccuracy: PositionModel;

  public positionLowAccuracy: PositionModel;

  public timestamp: Date;

  constructor(
    measurements: any[],
    positionLowAccuracy?: Position,
    positionHighAccuracy?: Position,
  ) {
    this.measurements = [];
    for (let i = 0; i < measurements.length; i++) {
      this.measurements[i] = {
        ssid: measurements[i].SSID,
        mac: measurements[i].BSSID,
        rssi: measurements[i].level,
      };
    }
    this.measurements = this.measurements.filter(m => m.ssid.includes('DHBW-KA')).sort((m1, m2) =>
      m2.rssi - m1.rssi);

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
  }
}
