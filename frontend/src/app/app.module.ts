import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouteReuseStrategy} from '@angular/router';

import {IonicModule, IonicRouteStrategy} from '@ionic/angular';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {WifiWizard2} from '@awesome-cordova-plugins/wifi-wizard-2/ngx';
import {Geolocation} from '@awesome-cordova-plugins/geolocation/ngx';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {IonicStorageModule} from "@ionic/storage-angular";
import {HttpClientModule} from "@angular/common/http";


@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    IonicModule.forRoot(),
    AppRoutingModule,
    BrowserAnimationsModule,
    IonicStorageModule.forRoot(),
    HttpClientModule
  ],
  providers: [
    {provide: RouteReuseStrategy, useClass: IonicRouteStrategy},
    WifiWizard2,
    Geolocation,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
}
