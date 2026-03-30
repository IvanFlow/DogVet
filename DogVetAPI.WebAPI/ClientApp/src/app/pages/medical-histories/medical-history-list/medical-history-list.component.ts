import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MedicalHistoryService } from '../../../services/medical-history.service';
import { OwnerService } from '../../../services/owner.service';
import { PetService } from '../../../services/pet.service';
import { MedicalHistory } from '../../../models/medical-history.model';
import { Pet } from '../../../models/pet.model';
import { Owner } from '../../../models/owner.model';
import { StatusPipe } from '../../../pipes/status.pipe';
import { SpanishDatePipe } from '../../../pipes/spanish-date.pipe';
import { ListStateService } from '../../../services/list-state.service';

@Component({
  selector: 'app-medical-history-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, StatusPipe, SpanishDatePipe],
  templateUrl: './medical-history-list.component.html'
})
export class MedicalHistoryListComponent implements OnInit, OnDestroy {
  records: MedicalHistory[] = [];
  pets: Pet[] = [];
  owners: Owner[] = [];
  search = '';
  filterOwner = '';
  filterPet = '';
  filterByFollowUp = false;
  followUpDateRange = '';
  loading = true;
  error: string | null = null;

  get filteredPets() {
    if (!this.filterOwner) return this.pets;
    return this.pets.filter(p => p.ownerId === Number(this.filterOwner));
  }

  get filtered() {
    const s = this.search.toLowerCase();
    const now = new Date();
    return this.records
      .filter(r => {
        const matchSearch = !s || r.diagnosis.toLowerCase().includes(s);
        const matchPet = !this.filterPet || r.petId === Number(this.filterPet);
        const ownerPetIds = this.filterOwner ? this.filteredPets.map(p => p.id) : null;
        const matchOwner = !ownerPetIds || ownerPetIds.includes(r.petId);
        
        let matchFollowUp = true;
        if (this.filterByFollowUp) {
          matchFollowUp = r.followUpDate != null && r.status !== 'Completed';
          if (matchFollowUp && this.followUpDateRange) {
            const followUpDate = new Date(r.followUpDate!);
            if (this.followUpDateRange === 'pending') {
              matchFollowUp = followUpDate > now && r.status !== 'Completed';
            } else if (this.followUpDateRange === 'overdue') {
              matchFollowUp = followUpDate < now && r.status !== 'Completed';
            } else {
              const days = Number(this.followUpDateRange);
              const futureDate = new Date(); futureDate.setDate(now.getDate() + days);
              matchFollowUp = followUpDate >= now && followUpDate <= futureDate && r.status !== 'Completed';
            }
          }
        }
        
        return matchSearch && matchPet && matchOwner && matchFollowUp;
      })
      .sort((a, b) => {
        if (this.filterByFollowUp) {
          return new Date(a.followUpDate || '').getTime()- new Date(b.followUpDate || '').getTime();
        }
        return new Date(b.visitDate).getTime() - new Date(a.visitDate).getTime();
      });
  }

  constructor(
    private medicalHistoryService: MedicalHistoryService,
    private petService: PetService,
    private ownerService: OwnerService,
    private router: Router,
    private listState: ListStateService
  ) {}

  navigateTo(id: number) {
    this.router.navigate(['/medical-histories', id]);
  }

  ngOnDestroy() {
    this.listState.medicalHistoryList = { search: this.search, filterOwner: this.filterOwner, filterPet: this.filterPet, filterByFollowUp: this.filterByFollowUp, followUpDateRange: this.followUpDateRange };
  }

  ngOnInit() {
    const shouldClearFilters = history.state?.clearFilters === true;

    if (shouldClearFilters) {
      this.search = '';
      this.filterOwner = '';
      this.filterPet = '';
      this.filterByFollowUp = history.state?.filterByFollowUp === true;
      this.followUpDateRange = history.state?.followUpDateRange || '';
      history.replaceState({ ...history.state, clearFilters: false }, '');
    } else {
      const s = this.listState.medicalHistoryList;
      this.search = s.search || '';
      this.filterOwner = s.filterOwner || '';
      this.filterPet = s.filterPet || '';
      this.filterByFollowUp = s.filterByFollowUp || false;
      this.followUpDateRange = s.followUpDateRange || '';
    }
    
    this.medicalHistoryService.getAll().subscribe({
      next: (data) => {
        this.records = data;
        this.loading = false;
        this.error = null;
      },
      error: (err) => {
        console.error('[MedicalHistoryList] Error:', err);
        this.error = 'Failed to load records.';
        this.loading = false;
      }
    });
    this.petService.getAll().subscribe(data => {
      this.pets = data;
    });
    this.ownerService.getAll().subscribe(data => {
      this.owners = data;
    });
  }

  onOwnerFilter() {
    this.filterPet = '';
  }

  getPetName(petId: number): string | null {
    return this.pets.find(p => p.id === petId)?.name ?? null;
  }
}
