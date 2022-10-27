import {Component, OnInit} from '@angular/core';
import {WifiWizard2} from "@awesome-cordova-plugins/wifi-wizard-2/ngx";
import {BehaviorSubject, from, Observable} from "rxjs";
import {first} from "rxjs/operators";

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss']
})
export class Tab1Page implements OnInit {

  scanResult: BehaviorSubject<any>;

  constructor(public wifiWizard: WifiWizard2) {}

  async ngOnInit(): Promise<any>{
    this.scanResult = new BehaviorSubject<any>({});

    await this.wifiWizard.requestPermission();
  }

  public async scanNetworks(): Promise<any> {
    console.log("scanning");
    await this.wifiWizard.scan().then(result => {
      this.scanResult.next(result);
      console.log(result);
    }).catch(reason => this.scanResult.next({reason}));
  }

}
