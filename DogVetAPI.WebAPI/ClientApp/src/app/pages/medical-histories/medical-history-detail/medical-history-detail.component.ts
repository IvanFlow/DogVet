import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { MedicalHistoryService } from '../../../services/medical-history.service';
import { PrescriptionService } from '../../../services/prescription.service';
import { MedicalHistory } from '../../../models/medical-history.model';
import { Prescription } from '../../../models/prescription.model';
import { Pet } from '../../../models/pet.model';
import { StatusPipe } from '../../../pipes/status.pipe';
import { DoseFrequencyPipe } from '../../../pipes/dose-frequency.pipe';
import { SpanishDatePipe } from '../../../pipes/spanish-date.pipe';
import { PrescriptionEditorModalComponent } from '../prescription-editor-modal/prescription-editor-modal.component';

@Component({
  selector: 'app-medical-history-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, StatusPipe, DoseFrequencyPipe, SpanishDatePipe, PrescriptionEditorModalComponent],
  templateUrl: './medical-history-detail.component.html'
})
export class MedicalHistoryDetailComponent implements OnInit {
  record?: MedicalHistory;
  pet?: Pet;
  followUpOfRecord?: MedicalHistory;
  prescriptions: Prescription[] = [];
  loading = true;
  error: string | null = null;

  @ViewChild(PrescriptionEditorModalComponent) prescriptionModal!: PrescriptionEditorModalComponent;

  constructor(
    private medicalHistoryService: MedicalHistoryService,
    private prescriptionService: PrescriptionService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  goBack() { this.location.back(); }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = Number(params.get('id'));
      this.loading = true;
      this.record = undefined;
      this.pet = undefined;
      this.followUpOfRecord = undefined;
      this.prescriptions = [];
      this.medicalHistoryService.getById(id).subscribe({
        next: (data) => {
          this.record = data;
          this.followUpOfRecord = data.followUpOfRecord ?? undefined;
          if (data.pet) {
            this.pet = data.pet;
          }
          if (data.prescriptions && data.prescriptions.length > 0) {
            this.prescriptions = data.prescriptions;
          } 
          
          this.loading = false;
          this.error = null;
        },
        error: (err) => {
          console.error('[MedicalHistoryDetail] Error:', err);
          this.error = 'Record not found.';
          this.loading = false;
        }
      });
    });
  }

  openPrescriptionEditor() {
    if (this.record) {
      this.prescriptionModal.open(this.record.id, this.prescriptions);
    }
  }

  delete() {
    if (!confirm('¿Deseas eliminar este registro médico?')) return;
    if (!this.record) return;
    
    this.medicalHistoryService.delete(this.record.id).subscribe({
      next: () => {
        console.log('[MedicalHistoryDetail] Deleted successfully');
        this.router.navigate(['/medical-histories']);
      },
      error: (err) => {
        console.error('[MedicalHistoryDetail] Delete error:', err);
        alert('Error al eliminar el registro.');
      }
    });
  }
}
