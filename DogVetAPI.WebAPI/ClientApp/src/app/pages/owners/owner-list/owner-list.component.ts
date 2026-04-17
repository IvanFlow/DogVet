import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
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

  private searchSubject = new Subject<string>();

  constructor(private ownerService: OwnerService, private router: Router, private listState: ListStateService) {}

  navigateTo(id: number) {
    this.router.navigate(['/owners', id]);
  }

  onSearchChange(value: string) {
    this.search = value;
    this.searchSubject.next(value);
  }

  ngOnDestroy() {
    this.listState.ownerList = { search: this.search };
    this.searchSubject.complete();
  }

  ngOnInit() {
    const shouldClearFilters = history.state?.clearFilters === true;

    if (shouldClearFilters) {
      this.search = '';
      history.replaceState({ ...history.state, clearFilters: false }, '');
    } else {
      this.search = this.listState.ownerList.search;
    }

    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(search => {
        this.loading = true;
        return this.ownerService.getFiltered(search);
      })
    ).subscribe({
      next: data => {
        this.owners = data;
        this.loading = false;
        this.error = null;
      },
      error: (err) => {
        this.error = `Failed to load owners: ${err?.message || 'Unknown error'}`;
        this.loading = false;
      }
    });

    // Initial load
    this.searchSubject.next(this.search);
  }
}
