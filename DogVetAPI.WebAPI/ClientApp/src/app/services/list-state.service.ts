import { Injectable } from '@angular/core';

export interface OwnerListState {
  search: string;
}

export interface PetListState {
  search: string;
  filterOwner: string;
}

export interface MedicalHistoryListState {
  search: string;
  filterOwner: string;
  filterPet: string;
}

@Injectable({ providedIn: 'root' })
export class ListStateService {
  ownerList: OwnerListState = { search: '' };
  petList: PetListState = { search: '', filterOwner: '' };
  medicalHistoryList: MedicalHistoryListState = { search: '', filterOwner: '', filterPet: '' };
}
