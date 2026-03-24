export interface MedicalHistory {
  id: number;
  diagnosis: string;
  notes: string;
  visitDate: string;
  followUpDate?: string | null;
  status: string;
  petId: number;
  petName?: string;
  ownerName?: string;
  veterinarianId: number;
  followUpOf?: number | null;
  followUpOfRecord?: MedicalHistory | null;
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
