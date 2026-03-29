import { Injectable } from '@angular/core';

export interface OwnerListState {
  search: string;
}

export interface PetListState {
  search: string;
  filterOwner: string;
  filterSpecies: string;
}

export interface MedicalHistoryListState {
  search: string;
  filterOwner: string;
  filterPet: string;
  filterByFollowUp?: boolean;
  followUpDateRange?: string;
}

@Injectable({ providedIn: 'root' })
export class ListStateService {
  ownerList: OwnerListState = { search: '' };
  petList: PetListState = { search: '', filterOwner: '', filterSpecies: '' };
  medicalHistoryList: MedicalHistoryListState = { search: '', filterOwner: '', filterPet: '', filterByFollowUp: false, followUpDateRange: '' };
}
