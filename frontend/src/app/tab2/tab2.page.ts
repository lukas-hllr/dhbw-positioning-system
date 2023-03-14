import {Component, AfterViewInit} from '@angular/core';
import * as L from 'leaflet';
import * as levels_geojson from '../../assets/map-levels/2og_cal.json';
import {ApiService} from "../services/api-service/api.service";
import {first} from "rxjs/operators";
import {PositionModel} from "../model/position.model";
import {ApScanService} from "../services/ap-scan-service/ap-scan.service";

@Component({
  selector: 'app-tab2',
  templateUrl: 'tab2.page.html',
  styleUrls: ['tab2.page.scss']
})
export class Tab2Page implements AfterViewInit {
  public clickedPos;
  private map;
  private position;
  private locationMarker;
  private locationAccuracy;

  private clickedMarker;

  constructor(private apiService: ApiService, private apScanService: ApScanService) {
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  private initMap(): void {
    L.Icon.Default.imagePath = 'assets/leaflet/';
    this.map = L.map('map', {
      center: [49.027184, 8.385406],
      zoom: 19
    });

    const tiles = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 25,
      minZoom: 3,
      attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    });

    tiles.addTo(this.map);

    L.geoJSON(levels_geojson as any).addTo(this.map);

    this.map.on('click', (e) => {
      if (this.clickedMarker !== undefined) {
        this.map.removeLayer(this.clickedMarker);
      }
      this.clickedMarker = L.marker(e.latlng).addTo(this.map);
      this.clickedMarker._icon.classList.add("debugMarker");
      this.clickedPos = e.latlng;
      console.log(e.latlng);
    });

    setTimeout(() => {
      this.map.invalidateSize();
    }, 0);
  }

  onMapReady(map: L.Map) {
    setTimeout(() => {
      map.invalidateSize();
    }, 0);
  }

  private refresh(): void {
    this.apScanService.scan().then(() => {
        this.apiService.getPosition(
          this.apScanService.scanResult.getValue().measurements
        ).pipe(first()).subscribe(
          position => this.setPosMarker(position)
        );
      }
    );
  }

  private setPosMarker(pos: PositionModel): void {
    if (this.locationMarker) this.map.removeLayer(this.locationMarker);
    if (this.locationAccuracy) this.map.removeLayer(this.locationAccuracy);

    this.locationMarker = L.marker([pos.latitude, pos.longitude]).addTo(this.map)
      .bindPopup('You are within ' + pos.accuracy + ' meters from this point').openPopup();

    this.locationAccuracy = L.circle([pos.latitude, pos.longitude], pos.accuracy / 2).addTo(this.map);
  }

  sendCurrentPos() {
    const realPos = new PositionModel(this.clickedPos.lat, this.clickedPos.lng, 0, 0);
    this.apScanService.send(realPos);
  }
}
