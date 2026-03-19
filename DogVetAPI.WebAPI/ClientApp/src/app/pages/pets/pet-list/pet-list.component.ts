import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PetService } from '../../../services/pet.service';
import { OwnerService } from '../../../services/owner.service';
import { Pet } from '../../../models/pet.model';
import { Owner } from '../../../models/owner.model';

@Component({
  selector: 'app-pet-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './pet-list.component.html'
})
export class PetListComponent implements OnInit {
  pets: Pet[] = [];
  owners: Owner[] = [];
  search = '';
  filterOwner = '';
  filterStatus = '';
  loading = true;
  error: string | null = null;

  get filtered() {
    return this.pets.filter(p => {
      const s = this.search.toLowerCase();
      const matchSearch = !s || p.name.toLowerCase().includes(s) || p.breed.toLowerCase().includes(s);
      const matchOwner = !this.filterOwner || p.ownerId === Number(this.filterOwner);
      const matchStatus = this.filterStatus === '' || String(p.isActive) === this.filterStatus;
      return matchSearch && matchOwner && matchStatus;
    });
  }

  constructor(private petService: PetService, private ownerService: OwnerService, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    console.log('[PetList] Loading...');
    this.petService.getAll().subscribe({
      next: (data) => {
        console.log('[PetList] Success:', data);
        this.pets = data;
        this.loading = false;
        this.error = null;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('[PetList] Error:', err);
        this.error = 'Failed to load pets.';
        this.loading = false;
        this.cdr.detectChanges();
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

  delete(pet: Pet) {
    if (!confirm(`Delete ${pet.name}?`)) return;
    this.petService.delete(pet.id).subscribe({
      next: () => this.pets = this.pets.filter(p => p.id !== pet.id),
      error: () => alert('Error deleting pet.')
    });
  }
}
