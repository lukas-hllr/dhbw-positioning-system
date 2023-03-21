import {PositionModel} from './position.model';
import {MeasurementModel} from './measurement.model';



export class MeasurementBackendModel extends  MeasurementModel{
  constructor(
    m: MeasurementModel,
    public positionGroundTruth: PositionModel,
    public device: string
) {
    super([]);
    this.measurements = m.measurements;
    this.positionLowAccuracy = m.positionLowAccuracy;
    this.positionHighAccuracy = m.positionHighAccuracy;
    this.timestamp = undefined;
  }
}
