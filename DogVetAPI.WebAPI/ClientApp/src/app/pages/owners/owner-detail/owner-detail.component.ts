import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { RouterLink, ActivatedRoute, Router } from '@angular/router';
import { OwnerService } from '../../../services/owner.service';
import { Owner } from '../../../models/owner.model';
import { PhonePipe } from '../../../pipes/phone.pipe';
import { AgePipe } from '../../../pipes/age.pipe';

@Component({
  selector: 'app-owner-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, PhonePipe, AgePipe],
  templateUrl: './owner-detail.component.html'
})
export class OwnerDetailComponent implements OnInit {
  owner?: Owner;
  loading = true;
  error: string | null = null;

  constructor(private ownerService: OwnerService, private route: ActivatedRoute, private router: Router, private location: Location) {}

  goBack() { this.location.back(); }

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.ownerService.getWithPets(id).subscribe({
      next: (data) => {
        this.owner = data;
        this.loading = false;
        this.error = null;
      },
      error: (err) => {
        console.error('[OwnerDetail] Error:', err);
        this.error = 'Owner not found.';
        this.loading = false;
      }
    });
  }

  delete() {
    if (!confirm(`¿Deseas eliminar a ${this.owner?.firstName} ${this.owner?.lastName}?`)) return;
    if (!this.owner) return;
    
    this.ownerService.delete(this.owner.id).subscribe({
      next: () => {
        this.router.navigate(['/owners']);
      },
      error: (err) => {
        console.error('[OwnerDetail] Delete error:', err);
        alert('Error al eliminar el propietario.');
      }
    });
  }
}
