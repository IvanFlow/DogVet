import { Pet } from './pet.model';

export interface MedicalHistory {
  id: number;
  diagnosis: string;
  notes: string;
  visitDate: string;
  followUpDate?: string | null;
  status: string;
  petId: number;
  veterinarianId: number;
  followUpOf?: number | null;
  followUpOfRecord?: MedicalHistory | null;
  pet?: Pet;
}

export interface CreateMedicalHistory {
  diagnosis: string;
  notes: string;
  visitDate: string;
  followUpDate?: string | null;
  petId: number;
  followUpOf?: number | null;
}

export interface UpdateMedicalHistory extends CreateMedicalHistory {
  id: number;
  status: string;
}
