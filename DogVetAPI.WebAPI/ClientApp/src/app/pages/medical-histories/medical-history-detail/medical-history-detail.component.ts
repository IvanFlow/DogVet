import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { MedicalHistoryService } from '../../../services/medical-history.service';
import { PrescriptionService } from '../../../services/prescription.service';
import { SaleNoteService, SaleNote } from '../../../services/sales-note.service';
import { MedicalHistory } from '../../../models/medical-history.model';
import { Prescription } from '../../../models/prescription.model';
import { Pet } from '../../../models/pet.model';
import { StatusPipe } from '../../../pipes/status.pipe';
import { DoseFrequencyPipe } from '../../../pipes/dose-frequency.pipe';
import { SpanishDatePipe } from '../../../pipes/spanish-date.pipe';
import { CurrencyMxPipe } from '../../../pipes/currency-mx.pipe';
import { PrescriptionEditorModalComponent } from '../prescription-editor-modal/prescription-editor-modal.component';
import { SalesNoteEditorModalComponent } from '../sales-note-editor-modal/sales-note-editor-modal.component';

@Component({
  selector: 'app-medical-history-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, StatusPipe, DoseFrequencyPipe, SpanishDatePipe, CurrencyMxPipe, PrescriptionEditorModalComponent, SalesNoteEditorModalComponent],
  templateUrl: './medical-history-detail.component.html'
})
export class MedicalHistoryDetailComponent implements OnInit {
  record?: MedicalHistory;
  pet?: Pet;
  followUpOfRecord?: MedicalHistory;
  prescriptions: Prescription[] = [];
  saleNotes: SaleNote[] = [];
  loading = true;
  error: string | null = null;

  @ViewChild(PrescriptionEditorModalComponent) prescriptionModal!: PrescriptionEditorModalComponent;
  @ViewChild(SalesNoteEditorModalComponent) salesNoteModal!: SalesNoteEditorModalComponent;

  constructor(
    private medicalHistoryService: MedicalHistoryService,
    private prescriptionService: PrescriptionService,
    private saleNoteService: SaleNoteService,
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
      this.saleNotes = [];
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
          this.saleNoteService.getByMedicalHistoryId(id).subscribe({
            next: (notes) => { this.saleNotes = notes; },
            error: () => { this.saleNotes = []; }
          });
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

  openSalesNoteEditor() {
    if (this.record) {
      this.salesNoteModal.open(this.record.id, this.prescriptions as any);
    }
  }

  openSaleNoteDetail(note: SaleNote) {
    this.router.navigate(['/sale-notes', note.id]);
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
