import { PositionModel } from "./position.model";

export class LocationModel extends PositionModel {

  constructor(
    public latitude: number,
    public longitude: number,
    public altitude: number,
    public accuracy: number,
    public room: number,
    public closestDoor: number,
  ) {
    super(longitude, latitude, altitude, accuracy);
  }
}
