import {Geoposition} from '@awesome-cordova-plugins/geolocation';



export class Meassurement {

  public meassurements: {
    SSID: string;
    MAC: string;
    level: number;
  }[];

  public positionLowAccuracy: {
    latitude: number;
    longitude: number;
    altitude: number;
    accuracy: number;
  };

  public positionHighAccuracy: {
    latitude: number;
    longitude: number;
    altitude: number;
    accuracy: number;
  };

  public timestamp: Date;

  constructor(
    meassurements: any[],
    positionLowAccuracy: Geoposition,
    positionHighAccuracy: Geoposition,
    ) {
    this.meassurements = [];
    for (let i = 0; i < meassurements.length; i++) {
      this.meassurements[i] = {
        SSID: meassurements[i].SSID,
        MAC: meassurements[i].BSSID,
        level: meassurements[i].level,
      };
    }
    this.meassurements = this.meassurements.filter(m => m.SSID === 'DHBW-KA').sort((m1, m2) =>
      m2.level - m1.level);

    this.positionHighAccuracy = {
      latitude: positionHighAccuracy.coords.latitude,
      longitude: positionHighAccuracy.coords.latitude,
      altitude: positionHighAccuracy.coords.altitude,
      accuracy: positionHighAccuracy.coords.accuracy
    };
    this.positionLowAccuracy = {
      latitude: positionLowAccuracy.coords.latitude,
      longitude: positionLowAccuracy.coords.latitude,
      altitude: positionLowAccuracy.coords.altitude,
      accuracy: positionLowAccuracy.coords.accuracy
    };
    this.timestamp = new Date(Date.now());
  }
}
