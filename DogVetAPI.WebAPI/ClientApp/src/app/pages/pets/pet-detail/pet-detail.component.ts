import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { PetService } from '../../../services/pet.service';
import { Pet } from '../../../models/pet.model';

@Component({
  selector: 'app-pet-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './pet-detail.component.html'
})
export class PetDetailComponent implements OnInit {
  pet?: Pet;
  loading = true;
  error: string | null = null;

  constructor(private petService: PetService, private route: ActivatedRoute, private router: Router, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    console.log('[PetDetail] Loading pet id:', id);
    this.petService.getById(id).subscribe({
      next: (data) => {
        console.log('[PetDetail] Success:', data);
        this.pet = data;
        this.loading = false;
        this.error = null;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('[PetDetail] Error:', err);
        this.error = 'Pet not found.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  delete() {
    if (!confirm(`¿Deseas eliminar a ${this.pet?.name}?`)) return;
    if (!this.pet) return;
    
    this.petService.delete(this.pet.id).subscribe({
      next: () => {
        console.log('[PetDetail] Deleted successfully');
        this.router.navigate(['/pets']);
      },
      error: (err) => {
        console.error('[PetDetail] Delete error:', err);
        alert('Error al eliminar la mascota.');
      }
    });
  }
}
