import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { SaleNoteService, SaleNote } from '../../../services/sales-note.service';
import { ListStateService } from '../../../services/list-state.service';
import { StatusPipe } from '../../../pipes/status.pipe';
import { SpanishDatePipe } from '../../../pipes/spanish-date.pipe';
import { CurrencyMxPipe } from '../../../pipes/currency-mx.pipe';

@Component({
  selector: 'app-sale-notes-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, StatusPipe, SpanishDatePipe, CurrencyMxPipe],
  templateUrl: './sale-notes-list.component.html'
})
export class SaleNotesListComponent implements OnInit, OnDestroy {
  saleNotes: SaleNote[] = [];
  search = '';
  filterStatus = '';
  loading = true;
  error: string | null = null;

  get filtered() {
    const s = this.search.toLowerCase();
    return this.saleNotes
      .filter(n => {
        const matchesSearch =
          `#${n.id}`.includes(s) ||
          (n.noteDate ?? '').toLowerCase().includes(s) ||
          (n.paymentStatus ?? '').toLowerCase().includes(s);
        const matchesStatus = !this.filterStatus || n.paymentStatus === this.filterStatus;
        return matchesSearch && matchesStatus;
      })
      .sort((a, b) => new Date(b.noteDate ?? 0).getTime() - new Date(a.noteDate ?? 0).getTime());
  }

  constructor(
    private saleNoteService: SaleNoteService,
    private router: Router,
    private listState: ListStateService
  ) {}

  navigateTo(id: number | undefined) {
    if (id != null) this.router.navigate(['/sale-notes', id]);
  }

  ngOnDestroy() {
    this.listState.saleNoteList = { search: this.search, filterStatus: this.filterStatus };
  }

  ngOnInit() {
    const shouldClearFilters = history.state?.clearFilters === true;

    if (shouldClearFilters) {
      this.search = '';
      this.filterStatus = history.state?.filterStatus || '';
      history.replaceState({ ...history.state, clearFilters: false }, '');
    } else {
      this.search = this.listState.saleNoteList.search;
      this.filterStatus = this.listState.saleNoteList.filterStatus;
    }

    this.saleNoteService.getAll().subscribe({
      next: (data) => {
        this.saleNotes = data;
        this.loading = false;
        this.error = null;
      },
      error: (err) => {
        console.error('[SaleNotesList] Error:', err);
        this.error = `Error al cargar las notas de venta: ${err?.message || 'Error desconocido'}`;
        this.loading = false;
      }
    });
  }
}
