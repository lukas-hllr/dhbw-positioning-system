import {Geoposition} from '@awesome-cordova-plugins/geolocation';
import {ApScanItemModel} from './ap-scan-item.model';
import {PositionModel} from './position.model';
import {MeasurementModel} from "./measurement.model";



export class MeasurementBackendModel extends  MeasurementModel{
  constructor(m: MeasurementModel, public positionGroundTruth: PositionModel) {
    super([]);
    this.measurements = m.measurements;
    this.positionHighAccuracy = m.positionHighAccuracy;
    this.positionLowAccuracy = m.positionLowAccuracy;
    this.positionHighAccuracy = m.positionHighAccuracy;
    this.device = m.device;
    this.timestamp = m.timestamp;
  }
}
