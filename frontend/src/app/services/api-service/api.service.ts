import {Injectable} from '@angular/core';
import {HttpClient, HttpResponse} from '@angular/common/http';
import {MeassurementModel} from '../../model/meassurement.model';
import {BehaviorSubject, Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {map} from 'rxjs/operators';
import {ApScanItemModel} from '../../model/ap-scan-item.model';
import {PositionModel} from "../../model/position.model";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  public $fetching: BehaviorSubject<boolean>;
  public $sending: BehaviorSubject<boolean>;

  constructor(private http: HttpClient) {
    this.$fetching = new BehaviorSubject(false);
    this.$sending = new BehaviorSubject(false);
  }

  public writeMeassurement(m: MeassurementModel): Observable<any> {
    this.$sending.next(true);
    return this.http.post<any>(`${environment.apiUrl}/`, m)
      .pipe(map(writtenMeassurement => {
        this.$sending.next(false);
        return writtenMeassurement;
      }));
  }

  public getMeassurement(m: MeassurementModel): Observable<any> {
    this.$sending.next(true);
    return this.http.post<any>(`${environment.apiUrl}/`, m)
      .pipe(map(writtenMeassurement => {
        this.$sending.next(false);
        return writtenMeassurement;
      }));
  }

  public getPosition(scan: ApScanItemModel[]): Observable<PositionModel> {
    this.$sending.next(true);
    return this.http.post<PositionModel>(`${environment.apiUrl}/location`, scan)
      .pipe(map(p => {
        this.$sending.next(false);
        return p;
      }));
  }

}
