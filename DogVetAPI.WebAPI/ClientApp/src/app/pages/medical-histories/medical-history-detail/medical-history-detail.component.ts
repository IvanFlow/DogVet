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
  templateUrl: './medical-history-detail.component.html',
  styles: [`
    .pets-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(180px, 1fr)); gap: var(--space-md); }
    .pet-card {
      border: 1px solid var(--color-border);
      border-radius: var(--radius-md);
      padding: var(--space-md);
      display: flex;
      flex-direction: column;
      gap: var(--space-xs);
      transition: box-shadow 0.2s, transform 0.2s;
    }
    .pet-card:hover { box-shadow: var(--shadow-md); transform: translateY(-2px); text-decoration: none; }
    .pet-name { font-weight: 600; color: var(--color-primary-dark); }
    .pet-meta { font-size: var(--font-size-sm); color: var(--color-text-muted); }
  `]
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
    this.route.paramMap.subscribe(params => {
      const id = Number(params.get('id'));
      this.loading = true;
      this.record = undefined;
      this.pet = undefined;
      this.followUpOfRecord = undefined;
      console.log('[MedicalHistoryDetail] Loading id:', id);
      this.medicalHistoryService.getById(id).subscribe({
        next: (data) => {
          console.log('[MedicalHistoryDetail] Success:', data);
          this.record = data;
          this.followUpOfRecord = data.followUpOfRecord ?? undefined;
          if (data.pet) {
            this.pet = data.pet;
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
