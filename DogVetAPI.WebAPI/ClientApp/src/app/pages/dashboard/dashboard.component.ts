import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OwnerService } from '../../services/owner.service';
import { PetService } from '../../services/pet.service';
import { MedicalHistoryService } from '../../services/medical-history.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent implements OnInit {
  ownerCount = 0;
  petCount = 0;
  recordCount = 0;

  constructor(
    private ownerService: OwnerService,
    private petService: PetService,
    private medicalHistoryService: MedicalHistoryService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    console.log('Dashboard loading...');
    this.ownerService.getAll().subscribe({
      next: o => { 
        console.log('Owners count:', o.length);
        this.ownerCount = o.length;
        this.cdr.markForCheck();
      },
      error: err => console.error('Error loading owner count:', err)
    });
    this.petService.getAll().subscribe({
      next: p => { 
        console.log('Pets count:', p.length);
        this.petCount = p.length;
        this.cdr.markForCheck();
      },
      error: err => console.error('Error loading pet count:', err)
    });
    this.medicalHistoryService.getAll().subscribe({
      next: r => { 
        console.log('Records count:', r.length);
        this.recordCount = r.length;
        this.cdr.markForCheck();
      },
      error: err => console.error('Error loading record count:', err)
    });
  }
}
