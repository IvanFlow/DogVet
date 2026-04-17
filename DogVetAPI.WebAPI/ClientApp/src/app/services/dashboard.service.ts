import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DashboardKpis {
  totalMedicalRecords: number;
  recentMedicalRecords: number;
  upcomingFollowUps: number;
  missedFollowUps: number;
  overdueFollowUps: number;
  totalOwners: number;
  totalPets: number;
  recentSaleNotes: number;
  pendingSaleNotes: number;
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = '/api/dashboard';

  constructor(private http: HttpClient) {}

  getKpis(): Observable<DashboardKpis> {
    return this.http.get<DashboardKpis>(`${this.apiUrl}/GetKpis`);
  }
}
