import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { PetService } from '../../../services/pet.service';
import { OwnerService } from '../../../services/owner.service';
import { Owner } from '../../../models/owner.model';

@Component({
  selector: 'app-pet-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './pet-form.component.html'
})
export class PetFormComponent implements OnInit {
  form!: FormGroup;
  isEdit = false;
  petId?: number;
  owners: Owner[] = [];
  saving = false;
  error: string | null = null;
  lockedOwnerId: number | null = null;
  lockedOwnerName = '';

  constructor(
    private fb: FormBuilder,
    private petService: PetService,
    private ownerService: OwnerService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      name:        ['', Validators.required],
      breed:       ['', Validators.required],
      gender:      ['', Validators.required],
      color:       [''],
      weight:      [0, [Validators.required, Validators.min(0)]],
      dateOfBirth: [''],
      ownerId:     ['', Validators.required],
      isActive:    [true]
    });

    const preselectedOwnerId = Number(this.route.snapshot.queryParamMap.get('ownerId')) || null;
    if (preselectedOwnerId) {
      this.lockedOwnerId = preselectedOwnerId;
      this.form.patchValue({ ownerId: preselectedOwnerId });
    }

    this.ownerService.getAll().subscribe(data => {
      this.owners = data;
      if (this.lockedOwnerId) {
        const owner = data.find(o => o.id === this.lockedOwnerId);
        if (owner) this.lockedOwnerName = `${owner.firstName} ${owner.lastName}`;
      }
    });

    this.petId = Number(this.route.snapshot.paramMap.get('id')) || undefined;
    this.isEdit = !!this.petId;

    if (this.isEdit && this.petId) {
      this.petService.getById(this.petId).subscribe({
        next: (pet) => {
          this.form.patchValue({
            ...pet,
            dateOfBirth: pet.dateOfBirth?.substring(0, 10) ?? ''
          });
          this.error = null;
        },
        error: (err) => {
          console.error('[PetForm] Error loading:', err);
          this.error = 'Failed to load pet.';
        }
      });
    }
  }

  submit() {
    if (this.form.invalid) return;
    this.saving = true;
    const value = this.form.value;
    value.ownerId = Number(value.ownerId);

    const req = this.isEdit && this.petId
      ? this.petService.update(this.petId, { ...value, id: this.petId })
      : this.petService.create(value);

    req.subscribe({
      next: pet => this.router.navigate(['/pets', pet.id]),
      error: () => { this.error = 'Failed to save pet.'; this.saving = false; }
    });
  }
}
