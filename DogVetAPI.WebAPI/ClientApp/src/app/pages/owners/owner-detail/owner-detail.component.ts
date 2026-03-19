import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { OwnerService } from '../../../services/owner.service';
import { Owner } from '../../../models/owner.model';

@Component({
  selector: 'app-owner-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './owner-detail.component.html',
  styles: [`
    .pets-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(180px, 1fr)); gap: var(--space-md); }
    .pet-card {
      border: 1px solid var(--color-border);
      border-radius: var(--radius-md);
      padding: var(--space-md);
      display: flex;
      flex-direction: column;
      gap: var(--space-xs);
      transition: box-shadow 0.2s, transform 0.2s;
    }
    .pet-card:hover { box-shadow: var(--shadow-md); transform: translateY(-2px); text-decoration: none; }
    .pet-name { font-weight: 600; color: var(--color-primary-dark); }
    .pet-meta { font-size: var(--font-size-sm); color: var(--color-text-muted); }
  `]
})
export class OwnerDetailComponent implements OnInit {
  owner?: Owner;
  loading = true;
  error: string | null = null;

  constructor(private ownerService: OwnerService, private route: ActivatedRoute, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    console.log('[OwnerDetail] Loading owner id:', id);
    this.ownerService.getWithPets(id).subscribe({
      next: (data) => {
        console.log('[OwnerDetail] Success:', data);
        this.owner = data;
        this.loading = false;
        this.error = null;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('[OwnerDetail] Error:', err);
        this.error = 'Owner not found.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
