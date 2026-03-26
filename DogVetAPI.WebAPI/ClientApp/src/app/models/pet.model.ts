import { MedicalHistory } from './medical-history.model';

export interface Pet {
  id: number;
  name: string;
  breed: string;
  weight: number;
  color: string;
  gender: string;
  dateOfBirth: string;
  species?: string;
  isActive: boolean;
  ownerId: number;
  ownerName?: string;
  createdAt: string;
  updatedAt: string;
  medicalHistories?: MedicalHistory[];
}

export interface CreatePet {
  name: string;
  breed: string;
  weight: number;
  color: string;
  gender: string;
  dateOfBirth: string;
  species?: string;
  ownerId: number;
}

export interface UpdatePet extends CreatePet {
  id: number;
  isActive: boolean;
}
