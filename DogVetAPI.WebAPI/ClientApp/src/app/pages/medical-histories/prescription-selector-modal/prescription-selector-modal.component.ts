import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Prescription } from '../../../models/prescription.model';

export interface PrescriptionWithSelected extends Prescription {
  selected: boolean;
}

@Component({
  selector: 'app-prescription-selector-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './prescription-selector-modal.component.html',
  styleUrls: ['./prescription-selector-modal.component.css']
})
export class PrescriptionSelectorModalComponent {
  isOpen = false;
  isMobile = window.innerWidth < 768;
  prescriptions: PrescriptionWithSelected[] = [];

  onContinue: ((selected: PrescriptionWithSelected[]) => void) | null = null;

  constructor() {
    window.addEventListener('resize', () => {
      this.isMobile = window.innerWidth < 768;
    });
  }

  get selectedCount(): number {
    return this.prescriptions.filter(p => p.selected).length;
  }

  open(prescriptions: Prescription[], callback: (selected: PrescriptionWithSelected[]) => void): void {
    this.onContinue = callback;
    // Filter prescriptions that are NOT 'Administered' and mark as unselected
    this.prescriptions = prescriptions
      .filter(p => p.status !== 'Administered')
      .map(p => ({ ...p, selected: false }));
    this.isOpen = true;
  }

  toggleSelection(prescription: PrescriptionWithSelected): void {
    prescription.selected = !prescription.selected;
  }

  continue(): void {
    const selectedPrescriptions = this.prescriptions.filter(p => p.selected);
    if (this.onContinue) {
      this.onContinue(selectedPrescriptions);
    }
    this.close();
  }

  close(): void {
    this.isOpen = false;
    this.prescriptions = [];
  }
}
