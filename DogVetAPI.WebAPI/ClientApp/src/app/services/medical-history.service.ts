import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MedicalHistory, CreateMedicalHistory, UpdateMedicalHistory } from '../models/medical-history.model';

@Injectable({ providedIn: 'root' })
export class MedicalHistoryService {
  private readonly baseUrl = '/api/medicalhistories';

  constructor(private http: HttpClient) {}

  getAll(): Observable<MedicalHistory[]> {
    return this.http.get<MedicalHistory[]>(this.baseUrl);
  }

  getById(id: number): Observable<MedicalHistory> {
    return this.http.get<MedicalHistory>(`${this.baseUrl}/${id}`);
  }

  getByPet(petId: number): Observable<MedicalHistory[]> {
    return this.http.get<MedicalHistory[]>(`${this.baseUrl}/pet/${petId}`);
  }

  getByVeterinarian(vetId: number): Observable<MedicalHistory[]> {
    return this.http.get<MedicalHistory[]>(`${this.baseUrl}/veterinarian/${vetId}`);
  }

  getByDateRange(startDate: string, endDate: string): Observable<MedicalHistory[]> {
    const params = new HttpParams().set('startDate', startDate).set('endDate', endDate);
    return this.http.get<MedicalHistory[]>(`${this.baseUrl}/by-date-range`, { params });
  }

  create(record: CreateMedicalHistory): Observable<MedicalHistory> {
    return this.http.post<MedicalHistory>(this.baseUrl, record);
  }

  update(id: number, record: UpdateMedicalHistory): Observable<MedicalHistory> {
    return this.http.put<MedicalHistory>(`${this.baseUrl}/${id}`, record);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
