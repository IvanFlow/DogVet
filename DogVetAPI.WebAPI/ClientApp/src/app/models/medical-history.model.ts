import { Pet } from './pet.model';
import { Prescription } from './prescription.model';

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
  prescriptions?: Prescription[];
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
