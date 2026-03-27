import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MedicalHistory, CreateMedicalHistory, UpdateMedicalHistory } from '../models/medical-history.model';

@Injectable({ providedIn: 'root' })
export class MedicalHistoryService {
  private readonly baseUrl = '/api/medicalhistories';

  constructor(private http: HttpClient) {}

  getAll(): Observable<MedicalHistory[]> {
    return this.http.get<MedicalHistory[]>(`${this.baseUrl}/GetAllRecords`);
  }

  getById(id: number): Observable<MedicalHistory> {
    return this.http.get<MedicalHistory>(`${this.baseUrl}/GetRecordById`, { params: { id } });
  }

  getByPetId(petId: number): Observable<MedicalHistory[]> {
    return this.http.get<MedicalHistory[]>(`${this.baseUrl}/GetRecordsByPetId`, { params: { petId } });
  }

  create(record: CreateMedicalHistory): Observable<MedicalHistory> {
    return this.http.post<MedicalHistory>(`${this.baseUrl}/CreateRecord`, record);
  }

  update(record: UpdateMedicalHistory): Observable<MedicalHistory> {
    return this.http.put<MedicalHistory>(`${this.baseUrl}/UpdateRecord`, record);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/SoftDeleteRecord`, { params: { id } });
  }
}
