import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DashboardService } from '../../services/dashboard.service';

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
  pendingSaleNotes = 0;

  constructor(private dashboardService: DashboardService) {}

  ngOnInit() {
    this.dashboardService.getKpis().subscribe({
      next: kpis => {
        this.recordCount = kpis.totalMedicalRecords;
        this.recentRecords = kpis.recentMedicalRecords;
        this.upcomingFollowUps = kpis.upcomingFollowUps;
        this.missedFollowUps = kpis.missedFollowUps;
        this.overdueFollowUps = kpis.overdueFollowUps;
        this.ownerCount = kpis.totalOwners;
        this.petCount = kpis.totalPets;
        this.recentSaleNotes = kpis.recentSaleNotes;
        this.pendingSaleNotes = kpis.pendingSaleNotes;
      },
      error: err => console.error('Error loading dashboard KPIs:', err)
    });
  }
}
