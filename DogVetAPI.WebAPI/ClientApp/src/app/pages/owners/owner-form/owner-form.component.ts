import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { OwnerService } from '../../../services/owner.service';
import { PhoneFormatterDirective } from '../../../directives/phone-formatter.directive';
import { phoneValidator } from '../../../validators/phone.validator';

@Component({
  selector: 'app-owner-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, PhoneFormatterDirective],
  templateUrl: './owner-form.component.html'
})
export class OwnerFormComponent implements OnInit {
  form!: FormGroup;
  isEdit = false;
  ownerId?: number;
  saving = false;
  error: string | null = null;

  constructor(
    private fb: FormBuilder,
    private ownerService: OwnerService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName:  ['', Validators.required],
      email:     ['', [Validators.required, Validators.email]],
      phoneNumber: ['', phoneValidator()],
      address:   [''],
      city:      ['']
    });

    this.ownerId = Number(this.route.snapshot.paramMap.get('id')) || undefined;
    this.isEdit = !!this.ownerId;

    if (this.isEdit && this.ownerId) {
      this.ownerService.getById(this.ownerId).subscribe({
        next: (owner) => {
          this.form.patchValue(owner);
          this.error = null;
        },
        error: (err) => {
          console.error('[OwnerForm] Error loading:', err);
          this.error = 'Failed to load owner.';
        }
      });
    }
  }

  submit() {
    if (this.form.invalid) {
      return;
    }
    this.saving = true;
    const value = this.form.value;

    if (value.phoneNumber) {
      value.phoneNumber = value.phoneNumber.replace(/\D/g, '');
    }

    const req = this.isEdit && this.ownerId
      ? this.ownerService.update({ ...value, id: this.ownerId })
      : this.ownerService.create(value);

    req.subscribe({
      next: owner => this.router.navigate(['/owners', owner.id]),
      error: () => { this.error = 'Failed to save owner.'; this.saving = false; }
    });
  }
}
