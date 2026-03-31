export interface Prescription {
  id: number;
  medName: string;
  dose: string;
  durationInDays: number;
  status: string;
  medicalHistoryId: number;
}
