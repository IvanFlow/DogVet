import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phone',
  standalone: true
})
export class PhonePipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) return '';
    
    // Remove all non-digits
    const digits = value.replace(/\D/g, '');
    
    // Format as XX-XX-XX-XX-XX (2-2-2-2-2)
    if (digits.length === 10) {
      return digits.slice(0, 2) + '-' + 
             digits.slice(2, 4) + '-' + 
             digits.slice(4, 6) + '-' + 
             digits.slice(6, 8) + '-' + 
             digits.slice(8, 10);
    }
    
    // If not 10 digits, return original value
    return value;
  }
}
