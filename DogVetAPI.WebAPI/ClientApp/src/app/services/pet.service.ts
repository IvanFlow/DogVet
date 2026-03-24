import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pet, CreatePet, UpdatePet } from '../models/pet.model';

@Injectable({ providedIn: 'root' })
export class PetService {
  private readonly baseUrl = '/api/pets';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Pet[]> {
    return this.http.get<Pet[]>(this.baseUrl);
  }

  getById(id: number): Observable<Pet> {
    return this.http.get<Pet>(`${this.baseUrl}/${id}`);
  }

  getWithHistory(id: number): Observable<Pet> {
    return this.http.get<Pet>(`${this.baseUrl}/with-history/${id}`);
  }

  create(pet: CreatePet): Observable<Pet> {
    return this.http.post<Pet>(this.baseUrl, pet);
  }

  update(id: number, pet: UpdatePet): Observable<Pet> {
    return this.http.put<Pet>(`${this.baseUrl}/${id}`, pet);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/soft-delete/${id}`);
  }
}
