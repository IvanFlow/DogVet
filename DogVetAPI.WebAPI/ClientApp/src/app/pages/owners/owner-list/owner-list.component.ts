import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OwnerService } from '../../../services/owner.service';
import { Owner } from '../../../models/owner.model';

@Component({
  selector: 'app-owner-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './owner-list.component.html'
})
export class OwnerListComponent implements OnInit {
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

constructor(private ownerService: OwnerService, private cdr: ChangeDetectorRef, private router: Router) {}

  navigateTo(id: number) {
    this.router.navigate(['/owners', id]);
  }

  ngOnInit() {
    console.log('[OwnerList] Initializing, about to call getAll()');
    this.ownerService.getAll().subscribe({
      next: (data) => {
        console.log('[OwnerList] Success! Data:', data);
        this.owners = data;
        this.loading = false;
        this.error = null;
        this.cdr.detectChanges(); 
        console.log('[OwnerList] loading set to false');
      },
      error: (err) => {
        console.error('[OwnerList] Error callback triggered:', err);
        this.error = `Failed to load owners: ${err?.message || 'Unknown error'}`;
        this.loading = false;
        this.cdr.detectChanges();
        console.log('[OwnerList] loading set to false due to error');
      },
      complete: () => {
        console.log('[OwnerList] Complete callback triggered');
      }
    });
  }
}
