import {Component, OnInit} from '@angular/core';
import {WifiWizard2} from "@awesome-cordova-plugins/wifi-wizard-2/ngx";
import {StorageService} from "./services/storage-service/storage.service";

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss'],
})
export class AppComponent implements OnInit{
  constructor(
    private wifiWizard: WifiWizard2,
    private storageService: StorageService
  ) {}

  async ngOnInit(): Promise<void> {
    await this.storageService.init();
    await this.wifiWizard.requestPermission();
  }
}
