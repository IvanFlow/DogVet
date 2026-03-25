import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OwnerService } from '../../../services/owner.service';
import { Owner } from '../../../models/owner.model';
import { PhonePipe } from '../../../pipes/phone.pipe';
import { ListStateService } from '../../../services/list-state.service';

@Component({
  selector: 'app-owner-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, PhonePipe],
  templateUrl: './owner-list.component.html'
})
export class OwnerListComponent implements OnInit, OnDestroy {
  owners: Owner[] = [];
  search = '';
  loading = true;
  error: string | null = null;

  get filtered() {
    const s = this.search.toLowerCase();
    return this.owners.filter(o =>
      `${o.firstName} ${o.lastName}`.toLowerCase().includes(s) ||
      o.email.toLowerCase().includes(s) ||
      o.city.toLowerCase().includes(s)
    );
  }

constructor(private ownerService: OwnerService, private router: Router, private listState: ListStateService) {}

  navigateTo(id: number) {
    this.router.navigate(['/owners', id]);
  }

  ngOnDestroy() {
    this.listState.ownerList = { search: this.search };
  }

  ngOnInit() {
    const shouldClearFilters = history.state?.clearFilters === true;
    
    if (shouldClearFilters) {
      this.search = '';
      history.replaceState({ ...history.state, clearFilters: false }, '');
    } else {
      this.search = this.listState.ownerList.search;
    }
    
    console.log('[OwnerList] Initializing, about to call getAll()');
    this.ownerService.getAll().subscribe({
      next: (data) => {
        console.log('[OwnerList] Success! Data:', data);
        this.owners = data;
        this.loading = false;
        this.error = null;
        console.log('[OwnerList] loading set to false');
      },
      error: (err) => {
        console.error('[OwnerList] Error callback triggered:', err);
        this.error = `Failed to load owners: ${err?.message || 'Unknown error'}`;
        this.loading = false;
        console.log('[OwnerList] loading set to false due to error');
      },
      complete: () => {
        console.log('[OwnerList] Complete callback triggered');
      }
    });
  }
}
