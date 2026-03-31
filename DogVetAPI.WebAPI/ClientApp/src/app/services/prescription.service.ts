import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Prescription } from '../models/prescription.model';

@Injectable({
  providedIn: 'root'
})
export class PrescriptionService {
  private apiUrl = '/api/prescriptions';

  constructor(private http: HttpClient) {}

  getByMedicalHistoryId(medicalHistoryId: number): Observable<Prescription[]> {
    return this.http.get<Prescription[]>(`${this.apiUrl}/GetByMedicalHistoryId?medicalHistoryId=${medicalHistoryId}`);
  }

  createPrescriptions(medicalHistoryId: number, prescriptions: any[]): Observable<Prescription[]> {
    const request = {
      medicalHistoryId,
      prescriptions
    };
    return this.http.post<Prescription[]>(`${this.apiUrl}/CreatePrescriptionsByMedicalHistoryId`, request);
  }

  getDoseFrequencyOptions(): Observable<{ value: string; id: number }[]> {
    return this.http.get<{ value: string; id: number }[]>(`${this.apiUrl}/GetDoseFrequencyOptions`);
  }

  getPrescriptionStatusOptions(): Observable<{ value: string; id: number }[]> {
    return this.http.get<{ value: string; id: number }[]>(`${this.apiUrl}/GetPrescriptionStatusOptions`);
  }
}
