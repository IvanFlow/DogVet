import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Owner, CreateOwner, UpdateOwner } from '../models/owner.model';

@Injectable({ providedIn: 'root' })
export class OwnerService {
  private readonly baseUrl = '/api/owners';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Owner[]> {
    return this.http.get<Owner[]>(`${this.baseUrl}/GetAllOwners`);
  }

  getById(id: number): Observable<Owner> {
    return this.http.get<Owner>(`${this.baseUrl}/GetOwnerById`, { params: { id } });
  }

  getWithPets(id: number): Observable<Owner> {
    return this.http.get<Owner>(`${this.baseUrl}/GetOwnerWithPets`, { params: { id } });
  }

  create(owner: CreateOwner): Observable<Owner> {
    return this.http.post<Owner>(`${this.baseUrl}/CreateOwner`, owner);
  }

  update(owner: UpdateOwner): Observable<Owner> {
    return this.http.put<Owner>(`${this.baseUrl}/UpdateOwner`, owner);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/SoftDeleteOwner`, { params: { id } });
  }
}
