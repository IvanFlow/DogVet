import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function phoneValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (!control.value) {
      return null; // Don't validate empty values
    }

    // Count only digits
    const digitCount = control.value.replace(/\D/g, '').length;

    // Must have exactly 8 digits
    if (digitCount !== 10) {
      return { invalidPhone: { value: control.value, required: 10, actual: digitCount } };
    }

    return null;
  };
}
