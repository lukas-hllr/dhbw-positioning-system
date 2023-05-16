export class MeasurementEntity {

  constructor(
    public ssid: string,
    public mac: string,
    public rssi: number,
    ) {}
}
