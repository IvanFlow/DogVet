import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Owner, CreateOwner, UpdateOwner } from '../models/owner.model';

@Injectable({ providedIn: 'root' })
export class OwnerService {
  private readonly baseUrl = '/api/owners';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Owner[]> {
    return this.http.get<Owner[]>(this.baseUrl);
  }

  getById(id: number): Observable<Owner> {
    return this.http.get<Owner>(`${this.baseUrl}/${id}`);
  }

  getWithPets(id: number): Observable<Owner> {
    return this.http.get<Owner>(`${this.baseUrl}/with-pets/${id}`);
  }

  create(owner: CreateOwner): Observable<Owner> {
    return this.http.post<Owner>(this.baseUrl, owner);
  }

  update(id: number, owner: UpdateOwner): Observable<Owner> {
    return this.http.put<Owner>(`${this.baseUrl}/${id}`, owner);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/soft-delete/${id}`);
  }
}
