import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { MedicalHistoryService } from '../../../services/medical-history.service';
import { PetService } from '../../../services/pet.service';
import { MedicalHistory } from '../../../models/medical-history.model';
import { Pet } from '../../../models/pet.model';
import { StatusPipe } from '../../../pipes/status.pipe';

@Component({
  selector: 'app-medical-history-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, StatusPipe],
  templateUrl: './medical-history-detail.component.html'
})
export class MedicalHistoryDetailComponent implements OnInit {
  record?: MedicalHistory;
  pet?: Pet;
  followUpOfRecord?: MedicalHistory;
  loading = true;
  error: string | null = null;

  constructor(
    private medicalHistoryService: MedicalHistoryService,
    private petService: PetService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  goBack() { this.location.back(); }

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    console.log('[MedicalHistoryDetail] Loading id:', id);
    this.medicalHistoryService.getById(id).subscribe({
      next: (data) => {
        console.log('[MedicalHistoryDetail] Success:', data);
        this.record = data;
        this.followUpOfRecord = data.followUpOfRecord ?? undefined;
        this.loading = false;
        this.error = null;
        this.petService.getById(data.petId).subscribe(p => { this.pet = p; });
      },
      error: (err) => {
        console.error('[MedicalHistoryDetail] Error:', err);
        this.error = 'Record not found.';
        this.loading = false;
      }
    });
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
