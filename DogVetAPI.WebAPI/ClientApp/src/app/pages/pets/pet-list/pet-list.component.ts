import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PetService } from '../../../services/pet.service';
import { OwnerService } from '../../../services/owner.service';
import { Pet } from '../../../models/pet.model';
import { Owner } from '../../../models/owner.model';
import { GenderPipe } from '../../../pipes/gender.pipe';

@Component({
  selector: 'app-pet-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, GenderPipe],
  templateUrl: './pet-list.component.html'
})
export class PetListComponent implements OnInit {
  pets: Pet[] = [];
  owners: Owner[] = [];
  search = '';
  filterOwner = '';
  loading = true;
  error: string | null = null;

  get filtered() {
    return this.pets.filter(p => {
      const s = this.search.toLowerCase();
      const matchSearch = !s || p.name.toLowerCase().includes(s) || p.breed.toLowerCase().includes(s);
      const matchOwner = !this.filterOwner || p.ownerId === Number(this.filterOwner);
      return matchSearch && matchOwner;
    });
  }

  constructor(private petService: PetService, private ownerService: OwnerService, private router: Router) {}

  navigateTo(id: number) {
    this.router.navigate(['/pets', id]);
  }

  ngOnInit() {
    console.log('[PetList] Loading...');
    this.petService.getAll().subscribe({
      next: (data) => {
        console.log('[PetList] Success:', data);
        this.pets = data;
        this.loading = false;
        this.error = null;
      },
      error: (err) => {
        console.error('[PetList] Error:', err);
        this.error = 'Failed to load pets.';
        this.loading = false;
      }
    });
    this.ownerService.getAll().subscribe({
      next: (data) => {
        console.log('[PetList] Owners:', data);
        this.owners = data;
      },
      error: (err) => console.error('[PetList] Owner error:', err)
    });
  }
}
