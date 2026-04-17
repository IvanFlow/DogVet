import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { PetService } from '../../../services/pet.service';
import { OwnerService } from '../../../services/owner.service';
import { Pet } from '../../../models/pet.model';
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
  ownerOptions: { id: number; name: string }[] = [];
  speciesOptions: string[] = [];
  search = '';
  filterOwner = '';
  filterSpecies = '';
  loading = true;
  error: string | null = null;

  private filterSubject = new Subject<{ search: string; filterOwner: string; filterSpecies: string }>();

  constructor(
    private petService: PetService,
    private ownerService: OwnerService,
    private router: Router,
    private listState: ListStateService
  ) {}

  navigateTo(id: number) {
    this.router.navigate(['/pets', id]);
  }

  onFilterChange() {
    this.filterSubject.next({ search: this.search, filterOwner: this.filterOwner, filterSpecies: this.filterSpecies });
  }

  ngOnDestroy() {
    this.listState.petList = { search: this.search, filterOwner: this.filterOwner, filterSpecies: this.filterSpecies };
    this.filterSubject.complete();
  }

  private refreshFilterOptions(pets: Pet[]) {
    const ownerMap = new Map<number, string>();
    const speciesSet = new Set<string>();
    for (const p of pets) {
      if (p.ownerId && p.ownerName) ownerMap.set(p.ownerId, p.ownerName);
      if (p.species) speciesSet.add(p.species);
    }
    this.ownerOptions = Array.from(ownerMap.entries())
      .map(([id, name]) => ({ id, name }))
      .sort((a, b) => a.name.localeCompare(b.name));
    this.speciesOptions = Array.from(speciesSet).sort();
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

    this.ownerService.getAll().subscribe({
      next: data => {
        this.ownerOptions = data.map(o => ({ id: o.id, name: `${o.firstName} ${o.lastName}` }))
          .sort((a, b) => a.name.localeCompare(b.name));
      },
      error: err => console.error('[PetList] Owner error:', err)
    });
    this.petService.getSpecies().subscribe({
      next: data => { this.speciesOptions = data.map(s => s.value).sort(); },
      error: err => console.error('[PetList] Species error:', err)
    });

    this.filterSubject.pipe(
      debounceTime(300),
      distinctUntilChanged((a, b) =>
        a.search === b.search && a.filterOwner === b.filterOwner && a.filterSpecies === b.filterSpecies
      ),
      switchMap(({ search, filterOwner, filterSpecies }) => {
        this.loading = true;
        const ownerId = filterOwner ? Number(filterOwner) : undefined;
        return this.petService.getFiltered(search, ownerId, filterSpecies);
      })
    ).subscribe({
      next: data => {
        this.pets = data;
        this.refreshFilterOptions(data);
        this.loading = false;
        this.error = null;
      },
      error: err => {
        console.error('[PetList] Error:', err);
        this.error = 'Error al cargar mascotas.';
        this.loading = false;
      }
    });

    // Initial load
    this.filterSubject.next({ search: this.search, filterOwner: this.filterOwner, filterSpecies: this.filterSpecies });
  }
}
