import {Injectable} from '@angular/core';
import {HttpClient, HttpResponse} from '@angular/common/http';
import {MeasurementModel} from '../../model/measurement.model';
import {BehaviorSubject, Observable} from 'rxjs';
import {environment} from '../../../environments/environment';
import {map} from 'rxjs/operators';
import {MeasurementEntity} from '../../model/measurement-entity';
import {PositionModel} from "../../model/position.model";
import {MeasurementBackendModel} from "../../model/measurement-backend.model";

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

  public writeMeasurement(m: MeasurementBackendModel): Observable<any> {
    this.$sending.next(true);
    return this.http.post<any>(`${environment.apiUrl}/measurement`, m)
      .pipe(map(writtenMeasurement => {
        this.$sending.next(false);
        return writtenMeasurement;
      }));
  }

  public getPosition(scan: MeasurementEntity[]): Observable<PositionModel> {
    this.$sending.next(true);
    return this.http.post<PositionModel>(`${environment.apiUrl}/location`, scan)
      .pipe(map(p => {
        this.$sending.next(false);
        return p;
      }));
  }

}
