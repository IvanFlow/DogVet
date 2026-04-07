import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PaymentStatusOption {
  value: string;
  id: number;
}

export interface SaleNoteConcept {
  id?: number;
  description: string;
  quantity: number;
  unitPrice: number;
  conceptPrice: number;
}

export interface SaleNote {
  id?: number;
  medicalHistoryId: number;
  noteDate?: string;
  totalAmount: number;
  paymentStatus?: string;
  concepts: SaleNoteConcept[];
}

export interface CreateSaleNoteRequest {
  medicalHistoryId: number;
  concepts: SaleNoteConcept[];
  prescriptionIds: number[];
}

@Injectable({
  providedIn: 'root'
})
export class SaleNoteService {
  private apiUrl = '/api/salenotes';

  constructor(private http: HttpClient) {}

  create(saleNote: SaleNote): Observable<SaleNote> {
    return this.http.post<SaleNote>(`${this.apiUrl}/CreateSaleNote`, saleNote);
  }

  getById(id: number): Observable<SaleNote> {
    return this.http.get<SaleNote>(`${this.apiUrl}/GetSaleNoteById`, { params: { id } });
  }

  getByMedicalHistoryId(medicalHistoryId: number): Observable<SaleNote[]> {
    return this.http.get<SaleNote[]>(`${this.apiUrl}/GetSaleNotesByMedicalHistoryId`, { params: { medicalHistoryId } });
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/DeleteSaleNote`, { params: { id } });
  }

  updatePaymentStatus(id: number, paymentStatus: string): Observable<SaleNote> {
    return this.http.patch<SaleNote>(`${this.apiUrl}/UpdatePaymentStatus`, null, { params: { id, paymentStatus } });
  }

  getPaymentStatuses(): Observable<PaymentStatusOption[]> {
    return this.http.get<PaymentStatusOption[]>(`${this.apiUrl}/GetPaymentStatuses`);
  }
}
