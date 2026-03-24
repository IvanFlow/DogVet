import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
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

  getByPetId(petId: number): Observable<MedicalHistory[]> {
    return this.http.get<MedicalHistory[]>(`${this.baseUrl}/pet/${petId}`);
  }

  create(record: CreateMedicalHistory): Observable<MedicalHistory> {
    return this.http.post<MedicalHistory>(this.baseUrl, record);
  }

  update(id: number, record: UpdateMedicalHistory): Observable<MedicalHistory> {
    return this.http.put<MedicalHistory>(`${this.baseUrl}/${id}`, record);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/soft-delete/${id}`);
  }
}
