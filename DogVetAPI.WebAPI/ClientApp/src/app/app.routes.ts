import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  {
    path: 'dashboard',
    loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent)
  },
  // Owners routes - most specific first
  {
    path: 'owners/new',
    loadComponent: () => import('./pages/owners/owner-form/owner-form.component').then(m => m.OwnerFormComponent)
  },
  {
    path: 'owners/:id/edit',
    loadComponent: () => import('./pages/owners/owner-form/owner-form.component').then(m => m.OwnerFormComponent)
  },
  {
    path: 'owners/:id',
    loadComponent: () => import('./pages/owners/owner-detail/owner-detail.component').then(m => m.OwnerDetailComponent)
  },
  {
    path: 'owners',
    loadComponent: () => import('./pages/owners/owner-list/owner-list.component').then(m => m.OwnerListComponent)
  },
  // Pets routes - most specific first
  {
    path: 'pets/new',
    loadComponent: () => import('./pages/pets/pet-form/pet-form.component').then(m => m.PetFormComponent)
  },
  {
    path: 'pets/:id/edit',
    loadComponent: () => import('./pages/pets/pet-form/pet-form.component').then(m => m.PetFormComponent)
  },
  {
    path: 'pets/:id',
    loadComponent: () => import('./pages/pets/pet-detail/pet-detail.component').then(m => m.PetDetailComponent)
  },
  {
    path: 'pets',
    loadComponent: () => import('./pages/pets/pet-list/pet-list.component').then(m => m.PetListComponent)
  },
  // Medical Histories routes - most specific first
  {
    path: 'medical-histories/new',
    loadComponent: () => import('./pages/medical-histories/medical-history-form/medical-history-form.component').then(m => m.MedicalHistoryFormComponent)
  },
  {
    path: 'medical-histories/:id/edit',
    loadComponent: () => import('./pages/medical-histories/medical-history-form/medical-history-form.component').then(m => m.MedicalHistoryFormComponent)
  },
  {
    path: 'medical-histories/:id',
    loadComponent: () => import('./pages/medical-histories/medical-history-detail/medical-history-detail.component').then(m => m.MedicalHistoryDetailComponent)
  },
  {
    path: 'medical-histories',
    loadComponent: () => import('./pages/medical-histories/medical-history-list/medical-history-list.component').then(m => m.MedicalHistoryListComponent)
  },
  // Sale Notes routes
  {
    path: 'sale-notes/:id',
    loadComponent: () => import('./pages/sale-notes/sale-note-detail/sale-note-detail.component').then(m => m.SaleNoteDetailComponent)
  },
  { path: '**', redirectTo: '/dashboard' }
];
