import { Component, Inject } from '@angular/core';
import { CommonModule, DOCUMENT } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { PrescriptionService } from '../../../services/prescription.service';
import { DoseFrequencyPipe } from '../../../pipes/dose-frequency.pipe';
import { StatusPipe } from '../../../pipes/status.pipe';

@Component({
  selector: 'app-prescription-editor-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, DoseFrequencyPipe, StatusPipe],
  templateUrl: './prescription-editor-modal.component.html',
  styleUrls: ['./prescription-editor-modal.component.css']
})
export class PrescriptionEditorModalComponent {
  form: FormGroup;
  medicalHistoryId: number = 0;
  isOpen = false;
  loading = false;
  error: string | null = null;
  isMobile = window.innerWidth < 768;
  doseFrequencyOptions: { value: string; id: number }[] = [];
  prescriptionStatusOptions: { value: string; id: number }[] = [];

  constructor(
    private fb: FormBuilder,
    private prescriptionService: PrescriptionService,
    @Inject(DOCUMENT) private document: Document
  ) {
    this.form = this.fb.group({
      prescriptions: this.fb.array([])
    });
    this.loadOptions();
    
    // Detect screen resize for responsive layout
    window.addEventListener('resize', () => {
      this.isMobile = window.innerWidth < 768;
    });
  }

  get prescriptionsArray(): FormArray {
    return this.form.get('prescriptions') as FormArray;
  }

  loadOptions() {
    this.prescriptionService.getDoseFrequencyOptions().subscribe({
      next: (options) => {
        this.doseFrequencyOptions = options;
      },
      error: (err) => {
        console.error('Error loading dose frequency options:', err);
      }
    });

    this.prescriptionService.getPrescriptionStatusOptions().subscribe({
      next: (options) => {
        this.prescriptionStatusOptions = options;
      },
      error: (err) => {
        console.error('Error loading prescription status options:', err);
      }
    });
  }

  open(medicalHistoryId: number, initialPrescriptions: any[] = []) {
    this.medicalHistoryId = medicalHistoryId;
    this.error = null;
    this.prescriptionsArray.clear();

    if (initialPrescriptions && initialPrescriptions.length > 0) {
      initialPrescriptions.forEach(prescription => {
        this.prescriptionsArray.push(
          this.fb.group({
            medName: [prescription.medName || '', Validators.required],
            dose: [prescription.dose || 'Daily', Validators.required],
            durationInDays: [prescription.durationInDays || 1, [Validators.required, Validators.min(1)]],
            status: [prescription.status || 'Prescribed', Validators.required]
          })
        );
      });
    } else {
      this.addPrescription();
    }

    this.isOpen = true;
    this.document.body.style.overflow = 'hidden';
  }

  addPrescription() {
    const defaultDose = this.doseFrequencyOptions.length > 0 ? this.doseFrequencyOptions[0].value : 'Daily';
    const defaultStatus = this.prescriptionStatusOptions.length > 0 ? this.prescriptionStatusOptions[0].value : 'Prescribed';

    this.prescriptionsArray.push(
      this.fb.group({
        medName: ['', Validators.required],
        dose: [defaultDose, Validators.required],
        durationInDays: [1, [Validators.required, Validators.min(1)]],
        status: [defaultStatus, Validators.required]
      })
    );
  }

  removePrescription(index: number) {
    this.prescriptionsArray.removeAt(index);
  }

  save() {
    if (this.prescriptionsArray.length === 0) {
      // Si no hay prescripciones, eliminar todas las existentes
      this.loading = true;
      this.prescriptionService.deleteAllPrescriptions(this.medicalHistoryId).subscribe({
        next: () => {
          this.loading = false;
          this.close();
          window.location.reload();
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Error al eliminar las prescripciones';
          console.error('Error deleting prescriptions:', err);
        }
      });
      return;
    }

    if (!this.form.valid) {
      this.error = 'Por favor, completa todos los campos requeridos';
      return;
    }

    this.loading = true;
    const prescriptions = this.prescriptionsArray.value;

    this.prescriptionService.createPrescriptions(this.medicalHistoryId, prescriptions).subscribe({
      next: () => {
        this.loading = false;
        this.close();
        window.location.reload();
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Error al guardar prescripciones';
        console.error('Error saving prescriptions:', err);
      }
    });
  }

  close() {
    this.isOpen = false;
    this.document.body.style.overflow = 'auto';
    this.form.reset();
    this.prescriptionsArray.clear();
  }
}
