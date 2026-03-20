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
}

export interface CreateMedicalHistory {
  diagnosis: string;
  notes: string;
  visitDate: string;
  followUpDate?: string | null;
  petId: number;
}

export interface UpdateMedicalHistory extends CreateMedicalHistory {
  id: number;
  status: string;
}
