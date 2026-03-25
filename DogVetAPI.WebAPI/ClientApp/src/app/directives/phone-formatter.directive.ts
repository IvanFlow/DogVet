import { Directive, HostListener } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appPhoneFormatter]',
  standalone: true
})
export class PhoneFormatterDirective {
  constructor(private ngControl: NgControl) {}

  @HostListener('input', ['$event'])
  onInput(event: any) {
    const input = event.target;
    let value = input.value.replace(/\D/g, ''); 

    // Limit to 10 digits
    if (value.length > 10) {
      value = value.slice(0, 10);
    }

    // Apply format: XX-XX-XX-XX-XX (23-24-25-26-27)
    if (value.length > 0) {
      if (value.length <= 2) {
        value = value;
      } else if (value.length <= 4) {
        value = value.slice(0, 2) + '-' + value.slice(2);
      } else if (value.length <= 6) {
        value = value.slice(0, 2) + '-' + value.slice(2, 4) + '-' + value.slice(4);
      } else if (value.length <= 8) {
        value = value.slice(0, 2) + '-' + value.slice(2, 4) + '-' + value.slice(4, 6) + '-' + value.slice(6, 8);
      } else if (value.length <= 10) {
        value = value.slice(0, 2) + '-' + value.slice(2, 4) + '-' + value.slice(4, 6) + '-' + value.slice(6, 8) + '-' + value.slice(8, 10);
      }
    }

    if (this.ngControl.control) {
      this.ngControl.control.setValue(value, { emitEvent: false });
    }
    input.value = value;
  }
}
