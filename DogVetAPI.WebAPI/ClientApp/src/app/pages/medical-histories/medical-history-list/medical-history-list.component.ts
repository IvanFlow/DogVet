import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MedicalHistoryService } from '../../../services/medical-history.service';
import { OwnerService } from '../../../services/owner.service';
import { PetService } from '../../../services/pet.service';
import { MedicalHistory } from '../../../models/medical-history.model';
import { Pet } from '../../../models/pet.model';
import { Owner } from '../../../models/owner.model';

@Component({
  selector: 'app-medical-history-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './medical-history-list.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MedicalHistoryListComponent implements OnInit {
  records: MedicalHistory[] = [];
  pets: Pet[] = [];
  owners: Owner[] = [];
  search = '';
  filterOwner = '';
  filterPet = '';
  loading = true;
  error: string | null = null;

  get filteredPets() {
    if (!this.filterOwner) return this.pets;
    return this.pets.filter(p => p.ownerId === Number(this.filterOwner));
  }

  get filtered() {
    const s = this.search.toLowerCase();
    return this.records.filter(r => {
      const matchSearch = !s || r.diagnosis.toLowerCase().includes(s);
      const matchPet = !this.filterPet || r.petId === Number(this.filterPet);
      const ownerPetIds = this.filterOwner ? this.filteredPets.map(p => p.id) : null;
      const matchOwner = !ownerPetIds || ownerPetIds.includes(r.petId);
      return matchSearch && matchPet && matchOwner;
    });
  }

  constructor(
    private medicalHistoryService: MedicalHistoryService,
    private petService: PetService,
    private ownerService: OwnerService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    console.log('[MedicalHistoryList] Loading...');
    this.medicalHistoryService.getAll().subscribe({
      next: (data) => {
        console.log('[MedicalHistoryList] Success:', data);
        this.records = data;
        this.loading = false;
        this.error = null;
        this.cdr.markForCheck();
      },
      error: (err) => {
        console.error('[MedicalHistoryList] Error:', err);
        this.error = 'Failed to load records.';
        this.loading = false;
        this.cdr.markForCheck();
      }
    });
    this.petService.getAll().subscribe(data => {
      this.pets = data;
      this.cdr.markForCheck();
    });
    this.ownerService.getAll().subscribe(data => {
      this.owners = data;
      this.cdr.markForCheck();
    });
  }

  onOwnerFilter() {
    this.filterPet = '';
  }

  getPetName(petId: number): string | null {
    return this.pets.find(p => p.id === petId)?.name ?? null;
  }
}
