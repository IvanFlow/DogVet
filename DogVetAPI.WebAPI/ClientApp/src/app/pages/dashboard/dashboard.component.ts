import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OwnerService } from '../../services/owner.service';
import { PetService } from '../../services/pet.service';
import { MedicalHistoryService } from '../../services/medical-history.service';
import { SaleNoteService } from '../../services/sales-note.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  ownerCount = 0;
  petCount = 0;
  recordCount = 0;
  upcomingFollowUps = 0;
  missedFollowUps = 0;
  overdueFollowUps = 0;
  recentRecords = 0;
  recentSaleNotes = 0;

  constructor(
    private ownerService: OwnerService,
    private petService: PetService,
    private medicalHistoryService: MedicalHistoryService,
    private saleNoteService: SaleNoteService
  ) {}

  ngOnInit() {
    this.ownerService.getAll().subscribe({
      next: o => { 
        this.ownerCount = o.length;
      },
      error: err => console.error('Error loading owner count:', err)
    });
    this.petService.getAll().subscribe({
      next: p => { 
        this.petCount = p.length;
      },
      error: err => console.error('Error loading pet count:', err)
    });
    this.medicalHistoryService.getAll().subscribe({
      next: r => {
        const now = new Date();
        const in30 = new Date(); in30.setDate(now.getDate() + 30);
        const ago30 = new Date(); ago30.setDate(now.getDate() - 30);

        this.recordCount = r.length;
        this.recentRecords = r.filter(rec => new Date(rec.visitDate) >= ago30).length;
        this.upcomingFollowUps = r.filter(rec =>
          rec.followUpDate &&
          new Date(rec.followUpDate) >= now &&
          new Date(rec.followUpDate) <= in30 &&
          rec.status !== 'Completed'
        ).length;
        this.missedFollowUps = r.filter(rec =>
          rec.followUpDate &&
          new Date(rec.followUpDate) > now &&
          rec.status !== 'Completed'
        ).length;
        this.overdueFollowUps = r.filter(rec =>
          rec.followUpDate &&
          new Date(rec.followUpDate) < now &&
          rec.status !== 'Completed'
        ).length;
      },
      error: err => console.error('Error loading record count:', err)
    });
    this.saleNoteService.getAll().subscribe({
      next: notes => {
        const now = new Date();
        const ago30 = new Date(now.getTime() - 30 * 24 * 60 * 60 * 1000);
        this.recentSaleNotes = notes.filter(n => {
          if (!n.noteDate) return false;
          const noteDate = new Date(n.noteDate);
          return noteDate.getTime() >= ago30.getTime();
        }).length;
      },
      error: err => console.error('Error loading sale notes count:', err)
    });
  }
}
