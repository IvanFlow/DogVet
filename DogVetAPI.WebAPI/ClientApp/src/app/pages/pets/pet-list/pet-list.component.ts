import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PetService } from '../../../services/pet.service';
import { OwnerService } from '../../../services/owner.service';
import { Pet } from '../../../models/pet.model';
import { Owner } from '../../../models/owner.model';
import { GenderPipe } from '../../../pipes/gender.pipe';
import { AgePipe } from '../../../pipes/age.pipe';
import { SpeciesPipe } from '../../../pipes/species.pipe';
import { ListStateService } from '../../../services/list-state.service';

@Component({
  selector: 'app-pet-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, GenderPipe, AgePipe, SpeciesPipe],
  templateUrl: './pet-list.component.html'
})
export class PetListComponent implements OnInit, OnDestroy {
  pets: Pet[] = [];
  owners: Owner[] = [];
  species: { value: string; id: number }[] = [];
  search = '';
  filterOwner = '';
  filterSpecies = '';
  loading = true;
  error: string | null = null;

  get filtered() {
    return this.pets
      .filter(p => {
        const s = this.search.toLowerCase();
        const matchSearch = !s || p.name.toLowerCase().includes(s) || p.breed.toLowerCase().includes(s);
        const matchOwner = !this.filterOwner || p.ownerId === Number(this.filterOwner);
        const matchSpecies = !this.filterSpecies || p.species === this.filterSpecies;
        return matchSearch && matchOwner && matchSpecies;
      })
      .sort((a, b) => a.name.localeCompare(b.name));
  }

  constructor(
    private petService: PetService,
    private ownerService: OwnerService,
    private router: Router,
    private listState: ListStateService
  ) {}

  navigateTo(id: number) {
    this.router.navigate(['/pets', id]);
  }

  ngOnDestroy() {
    this.listState.petList = { search: this.search, filterOwner: this.filterOwner, filterSpecies: this.filterSpecies };
  }

  ngOnInit() {
    const shouldClearFilters = history.state?.clearFilters === true;
    
    if (shouldClearFilters) {
      this.search = '';
      this.filterOwner = '';
      this.filterSpecies = '';
      history.replaceState({ ...history.state, clearFilters: false }, '');
    } else {
      const s = this.listState.petList;
      this.search = s.search;
      this.filterOwner = s.filterOwner;
      this.filterSpecies = s.filterSpecies || '';
    }
    
    this.petService.getAll().subscribe({
      next: (data) => {
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
        this.owners = data;
      },
      error: (err) => console.error('[PetList] Owner error:', err)
    });
    this.petService.getSpecies().subscribe({
      next: (data) => {
        this.species = data;
      },
      error: (err) => console.error('[PetList] Species error:', err)
    });
  }
}
