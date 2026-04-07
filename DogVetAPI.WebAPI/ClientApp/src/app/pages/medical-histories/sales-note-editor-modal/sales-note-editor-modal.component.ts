import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, FormArray } from '@angular/forms';
import { SaleNoteService, SaleNoteConcept, SaleNote } from '../../../services/sales-note.service';
import { PrescriptionSelectorModalComponent, PrescriptionWithSelected } from '../prescription-selector-modal/prescription-selector-modal.component';

@Component({
  selector: 'app-sales-note-editor-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, PrescriptionSelectorModalComponent],
  templateUrl: './sales-note-editor-modal.component.html',
  styleUrls: ['./sales-note-editor-modal.component.css']
})
export class SalesNoteEditorModalComponent implements OnInit {
  @ViewChild(PrescriptionSelectorModalComponent) prescriptionSelector!: PrescriptionSelectorModalComponent;

  isOpen = false;
  isMobile = window.innerWidth < 768;
  form!: FormGroup;
  loading = false;
  error = '';
  medicalHistoryId: number | null = null;
  selectedPrescriptions: PrescriptionWithSelected[] = [];

  constructor(
    private fb: FormBuilder,
    private saleNoteService: SaleNoteService
  ) {
    window.addEventListener('resize', () => {
      this.isMobile = window.innerWidth < 768;
    });
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      concepts: this.fb.array([])
    });
  }

  open(medicalHistoryId: number, prescriptions: import('../prescription-selector-modal/prescription-selector-modal.component').PrescriptionWithSelected[]): void {
    this.medicalHistoryId = medicalHistoryId;
    // Pass already-loaded prescriptions to the selector
    this.prescriptionSelector.open(prescriptions, (selected) => {
      this.selectedPrescriptions = selected;
      this.loadSelectedPrescriptions();
      this.isOpen = true;
    });
  }

  private loadSelectedPrescriptions(): void {
    const conceptsArray = this.form.get('concepts') as FormArray;
    conceptsArray.clear();

    this.selectedPrescriptions.forEach((prescription) => {
      const concept = this.fb.group({
        description: [prescription.medName],
        quantity: [1],
        unitPrice: [0],
        conceptPrice: [0]
      });

      // Update conceptPrice when quantity or unitPrice changes
      concept.valueChanges.subscribe(() => {
        const quantity = concept.get('quantity')?.value || 0;
        const unitPrice = concept.get('unitPrice')?.value || 0;
        const conceptPrice = quantity * unitPrice;
        concept.patchValue({ conceptPrice }, { emitEvent: false });
      });

      conceptsArray.push(concept);
    });
  }

  get conceptsArray(): FormArray {
    return this.form.get('concepts') as FormArray;
  }

  calculateTotal(): number {
    return this.conceptsArray.controls.reduce((sum, item) => {
      return sum + (item.get('conceptPrice')?.value || 0);
    }, 0);
  }

  addConcept(): void {
    const newConcept = this.fb.group({
      description: [''],
      quantity: [1],
      unitPrice: [0],
      conceptPrice: [0]
    });

    newConcept.valueChanges.subscribe(() => {
      const quantity = newConcept.get('quantity')?.value || 0;
      const unitPrice = newConcept.get('unitPrice')?.value || 0;
      const conceptPrice = quantity * unitPrice;
      newConcept.patchValue({ conceptPrice }, { emitEvent: false });
    });

    this.conceptsArray.push(newConcept);
  }

  removeConcept(index: number): void {
    this.conceptsArray.removeAt(index);
  }

  save(): void {
    if (!this.medicalHistoryId || this.conceptsArray.length === 0) {
      this.error = 'Debe agregar al menos un concepto a la nota de venta';
      return;
    }

    this.loading = true;
    this.error = '';

    const saleNote: SaleNote = {
      medicalHistoryId: this.medicalHistoryId!,
      totalAmount: this.calculateTotal(),
      concepts: this.conceptsArray.value as SaleNoteConcept[]
    };

    this.saleNoteService.create(saleNote).subscribe(
      (result) => {
        this.loading = false;
        this.close();
        // TODO: Navigate to sales note details or show success message
        window.location.reload();
      },
      (error) => {
        this.loading = false;
        this.error = error.error?.message || 'Error al crear la nota de venta';
      }
    );
  }

  close(): void {
    this.isOpen = false;
    this.form.reset();
    const conceptsArray = this.form.get('concepts') as FormArray;
    conceptsArray.clear();
    this.error = '';
  }
}
