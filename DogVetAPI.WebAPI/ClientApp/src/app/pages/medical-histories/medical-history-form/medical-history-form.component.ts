import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { MedicalHistoryService } from '../../../services/medical-history.service';
import { PetService } from '../../../services/pet.service';
import { OwnerService } from '../../../services/owner.service';
import { Pet } from '../../../models/pet.model';
import { Owner } from '../../../models/owner.model';
import { MedicalHistory } from '../../../models/medical-history.model';

@Component({
  selector: 'app-medical-history-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterLink],
  templateUrl: './medical-history-form.component.html'
})
export class MedicalHistoryFormComponent implements OnInit, OnDestroy {
  form!: FormGroup;
  isEdit = false;
  recordId?: number;
  pets: Pet[] = [];
  owners: Owner[] = [];
  availableFollowUpRecords: MedicalHistory[] = [];
  selectedOwner = '';
  saving = false;
  error: string | null = null;
  lockedPetId: number | null = null;
  lockedPetName = '';
  lockedOwnerName = '';
  private destroy$ = new Subject<void>();

  get availablePets() {
    if (!this.selectedOwner) return this.pets;
    return this.pets.filter(p => p.ownerId === Number(this.selectedOwner));
  }

  private resolveLockedOwner() {
    if (!this.lockedPetId || !this.pets.length || !this.owners.length) return;
    const pet = this.pets.find(p => p.id === this.lockedPetId);
    if (!pet) return;
    this.lockedPetName = `${pet.name} (${pet.breed})`;
    this.selectedOwner = String(pet.ownerId);
    const owner = this.owners.find(o => o.id === pet.ownerId);
    if (owner) this.lockedOwnerName = `${owner.firstName} ${owner.lastName}`;
  }

  constructor(
    private fb: FormBuilder,
    private medicalHistoryService: MedicalHistoryService,
    private petService: PetService,
    private ownerService: OwnerService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    const today = new Date().toISOString().substring(0, 10);
    this.form = this.fb.group({
      petId:       ['', Validators.required],
      diagnosis:   ['', Validators.required],
      notes:       [''],
      visitDate:   [today, Validators.required],
      followUpDate:[''],
      followUpOf:  [''],
      status:      ['Completed'],
    });

    const preselectedPetId = Number(this.route.snapshot.queryParamMap.get('petId')) || null;
    if (preselectedPetId) {
      this.lockedPetId = preselectedPetId;
      this.form.patchValue({ petId: preselectedPetId });
      this.loadRecordsForPet(preselectedPetId);
    }

    this.petService.getAll().subscribe(data => { this.pets = data; this.resolveLockedOwner(); });
    this.ownerService.getAll().subscribe(data => { this.owners = data; this.resolveLockedOwner(); });

    // Subscribe to petId changes to load records for that pet
    this.form.get('petId')?.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(petId => {
        if (petId) {
          this.loadRecordsForPet(Number(petId));
        } else {
          this.availableFollowUpRecords = [];
        }
      });

    this.recordId = Number(this.route.snapshot.paramMap.get('id')) || undefined;
    this.isEdit = !!this.recordId;

    if (this.isEdit && this.recordId) {
      this.medicalHistoryService.getById(this.recordId).subscribe({
        next: (record) => {
          this.form.patchValue({
            ...record,
            visitDate: record.visitDate?.substring(0, 10) ?? today,
            followUpDate: record.followUpDate?.substring(0, 10) ?? '',
            followUpOf: record.followUpOf ?? ''
          });
          this.error = null;
        },
        error: (err) => {
          console.error('[MedicalHistoryForm] Error loading:', err);
          this.error = 'Failed to load record.';
        }
      });
    }
  }

  private loadRecordsForPet(petId: number) {
    this.medicalHistoryService.getByPetId(petId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (records) => {
          this.availableFollowUpRecords = records.filter(r => 
            r.status === 'Completed' || r.status === 'Follow-up'
          );
        },
        error: (err) => {
          console.error('[MedicalHistoryForm] Error loading pet records:', err);
          this.availableFollowUpRecords = [];
        }
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onOwnerChange() {
    this.form.patchValue({ petId: '' });
    this.availableFollowUpRecords = [];
  }

  submit() {
    if (this.form.invalid) return;
    this.saving = true;
    const value = this.form.value;
    value.petId = Number(value.petId);
    if (!value.followUpDate) value.followUpDate = null;
    if (value.followUpOf) value.followUpOf = Number(value.followUpOf);
    else value.followUpOf = null;

    const req = this.isEdit && this.recordId
      ? this.medicalHistoryService.update(this.recordId, { ...value, id: this.recordId })
      : this.medicalHistoryService.create(value);

    req.subscribe({
      next: record => this.router.navigate(['/medical-histories', record.id]),
      error: () => { this.error = 'Failed to save record.'; this.saving = false; }
    });
  }
}
