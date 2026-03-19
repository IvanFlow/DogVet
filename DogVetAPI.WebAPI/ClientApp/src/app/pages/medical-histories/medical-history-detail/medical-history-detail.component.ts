import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { MedicalHistoryService } from '../../../services/medical-history.service';
import { PetService } from '../../../services/pet.service';
import { MedicalHistory } from '../../../models/medical-history.model';
import { Pet } from '../../../models/pet.model';

@Component({
  selector: 'app-medical-history-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './medical-history-detail.component.html'
})
export class MedicalHistoryDetailComponent implements OnInit {
  record?: MedicalHistory;
  pet?: Pet;
  loading = true;
  error: string | null = null;

  constructor(
    private medicalHistoryService: MedicalHistoryService,
    private petService: PetService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    console.log('[MedicalHistoryDetail] Loading id:', id);
    this.medicalHistoryService.getById(id).subscribe({
      next: (data) => {
        console.log('[MedicalHistoryDetail] Success:', data);
        this.record = data;
        this.loading = false;
        this.error = null;
        this.cdr.detectChanges();
        this.petService.getById(data.petId).subscribe(p => { this.pet = p; this.cdr.detectChanges(); });
      },
      error: (err) => {
        console.error('[MedicalHistoryDetail] Error:', err);
        this.error = 'Record not found.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
