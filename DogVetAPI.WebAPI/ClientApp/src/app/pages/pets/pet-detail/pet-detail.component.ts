import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { PetService } from '../../../services/pet.service';
import { Pet } from '../../../models/pet.model';
import { GenderPipe } from '../../../pipes/gender.pipe';
import { AgePipe } from '../../../pipes/age.pipe';
import { StatusPipe } from '../../../pipes/status.pipe';

@Component({
  selector: 'app-pet-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, GenderPipe, StatusPipe, AgePipe],
  templateUrl: './pet-detail.component.html'
})
export class PetDetailComponent implements OnInit {
  pet?: Pet;
  loading = true;
  error: string | null = null;

  constructor(private petService: PetService, private route: ActivatedRoute, private router: Router, private location: Location) {}

  goBack() { this.location.back(); }

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.petService.getWithHistory(id).subscribe({
      next: (data) => {
        this.pet = data;
        this.loading = false;
        this.error = null;
      },
      error: (err) => {
        console.error('[PetDetail] Error:', err);
        this.error = 'Pet not found.';
        this.loading = false;
      }
    });
  }

  delete() {
    if (!confirm(`¿Deseas eliminar a ${this.pet?.name}?`)) return;
    if (!this.pet) return;
    
    this.petService.delete(this.pet.id).subscribe({
      next: () => {
        this.router.navigate(['/pets']);
      },
      error: (err) => {
        console.error('[PetDetail] Delete error:', err);
        alert('Error al eliminar la mascota.');
      }
    });
  }
}
