import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pet, CreatePet, UpdatePet } from '../models/pet.model';

@Injectable({ providedIn: 'root' })
export class PetService {
  private readonly baseUrl = '/api/pets';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Pet[]> {
    return this.http.get<Pet[]>(`${this.baseUrl}/GetAllPets`);
  }

  getById(id: number): Observable<Pet> {
    return this.http.get<Pet>(`${this.baseUrl}/GetPetById`, { params: { id } });
  }

  getWithHistory(id: number): Observable<Pet> {
    return this.http.get<Pet>(`${this.baseUrl}/GetPetWithHistory`, { params: { id } });
  }

  create(pet: CreatePet): Observable<Pet> {
    return this.http.post<Pet>(`${this.baseUrl}/CreatePet`, pet);
  }

  update(pet: UpdatePet): Observable<Pet> {
    return this.http.put<Pet>(`${this.baseUrl}/UpdatePet`, pet);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/SoftDeletePet`, { params: { id } });
  }

  getSpecies(): Observable<{ value: string; id: number }[]> {
    return this.http.get<{ value: string; id: number }[]>(`${this.baseUrl}/GetPetSpecies`);
  }
}
