import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { MedicalHistoryService } from '../../../services/medical-history.service';
import { PetService } from '../../../services/pet.service';
import { OwnerService } from '../../../services/owner.service';
import { Pet } from '../../../models/pet.model';
import { Owner } from '../../../models/owner.model';

@Component({
  selector: 'app-medical-history-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule, RouterLink],
  templateUrl: './medical-history-form.component.html'
})
export class MedicalHistoryFormComponent implements OnInit {
  form!: FormGroup;
  isEdit = false;
  recordId?: number;
  pets: Pet[] = [];
  owners: Owner[] = [];
  selectedOwner = '';
  saving = false;
  error: string | null = null;

  get availablePets() {
    if (!this.selectedOwner) return this.pets;
    return this.pets.filter(p => p.ownerId === Number(this.selectedOwner));
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
      status:      ['Completed'],
    });

    this.petService.getAll().subscribe(data => { this.pets = data; });
    this.ownerService.getAll().subscribe(data => { this.owners = data; });

    const preselectedPet = this.route.snapshot.queryParamMap.get('petId');
    if (preselectedPet) {
      this.form.patchValue({ petId: Number(preselectedPet) });
      const pet = this.pets.find(p => p.id === Number(preselectedPet));
      if (pet) this.selectedOwner = String(pet.ownerId);
    }

    this.recordId = Number(this.route.snapshot.paramMap.get('id')) || undefined;
    this.isEdit = !!this.recordId;

    if (this.isEdit && this.recordId) {
      this.medicalHistoryService.getById(this.recordId).subscribe({
        next: (record) => {
          this.form.patchValue({
            ...record,
            visitDate: record.visitDate?.substring(0, 10) ?? today,
            followUpDate: record.followUpDate?.substring(0, 10) ?? ''
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

  onOwnerChange() {
    this.form.patchValue({ petId: '' });
  }

  submit() {
    if (this.form.invalid) return;
    this.saving = true;
    const value = this.form.value;
    value.petId = Number(value.petId);
    if (!value.followUpDate) value.followUpDate = null;

    const req = this.isEdit && this.recordId
      ? this.medicalHistoryService.update(this.recordId, { ...value, id: this.recordId })
      : this.medicalHistoryService.create(value);

    req.subscribe({
      next: record => this.router.navigate(['/medical-histories', record.id]),
      error: () => { this.error = 'Failed to save record.'; this.saving = false; }
    });
  }
}
