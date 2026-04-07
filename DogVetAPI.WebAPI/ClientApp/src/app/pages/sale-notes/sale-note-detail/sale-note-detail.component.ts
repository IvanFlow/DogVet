import { Component, OnInit } from '@angular/core';
import { CommonModule, Location } from '@angular/common';
import { RouterLink, Router, ActivatedRoute } from '@angular/router';
import { SaleNoteService, SaleNote, PaymentStatusOption } from '../../../services/sales-note.service';
import { StatusPipe } from '../../../pipes/status.pipe';
import { SpanishDatePipe } from '../../../pipes/spanish-date.pipe';

@Component({
  selector: 'app-sale-note-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, SpanishDatePipe, StatusPipe],
  templateUrl: './sale-note-detail.component.html'
})
export class SaleNoteDetailComponent implements OnInit {
  saleNote?: SaleNote;
  loading = true;
  error: string | null = null;
  updatingStatus = false;
  paymentStatuses: PaymentStatusOption[] = [];

  constructor(
    private router: Router,
    private location: Location,
    private saleNoteService: SaleNoteService,
    private activatedRoute: ActivatedRoute
  ) {}

  goBack() { this.location.back(); }

  ngOnInit() {
    this.saleNoteService.getPaymentStatuses().subscribe({
      next: (statuses) => { this.paymentStatuses = statuses; },
      error: () => { this.paymentStatuses = []; }
    });

    this.activatedRoute.params.subscribe(params => {
      const id = params['id'];
      if (id) {
        this.saleNoteService.getById(id).subscribe({
          next: (saleNote) => {
            this.saleNote = saleNote;
            this.loading = false;
          },
          error: () => {
            this.error = 'Error al cargar la nota de venta.';
            this.loading = false;
          }
        });
      } else {
        this.error = 'ID de nota de venta no especificado.';
        this.loading = false;
      }
    });
  }

  changeStatus(paymentStatus: string) {
    if (!this.saleNote?.id || this.saleNote.paymentStatus === paymentStatus) return;
    this.updatingStatus = true;
    this.saleNoteService.updatePaymentStatus(this.saleNote.id, paymentStatus).subscribe({
      next: (updated) => {
        this.saleNote = updated;
        this.updatingStatus = false;
      },
      error: () => {
        alert('Error al actualizar el estado de pago.');
        this.updatingStatus = false;
      }
    });
  }

  delete() {
    if (!confirm('¿Deseas eliminar esta nota de venta?')) return;
    if (!this.saleNote?.id) return;

    this.saleNoteService.delete(this.saleNote.id).subscribe({
      next: () => { this.location.back(); },
      error: () => { alert('Error al eliminar la nota de venta.'); }
    });
  }
}
