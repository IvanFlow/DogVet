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
import { ListStateService } from '../../../services/list-state.service';

@Component({
  selector: 'app-pet-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, GenderPipe, AgePipe],
  templateUrl: './pet-list.component.html'
})
export class PetListComponent implements OnInit, OnDestroy {
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
    this.listState.petList = { search: this.search, filterOwner: this.filterOwner };
  }

  ngOnInit() {
    const shouldClearFilters = history.state?.clearFilters === true;
    
    if (shouldClearFilters) {
      this.search = '';
      this.filterOwner = '';
      history.replaceState({ ...history.state, clearFilters: false }, '');
    } else {
      const s = this.listState.petList;
      this.search = s.search;
      this.filterOwner = s.filterOwner;
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
  }
}
