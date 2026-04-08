import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'currencyMx',
  standalone: true
})
export class CurrencyMxPipe implements PipeTransform {
  transform(value: number | null | undefined): string {
    if (value == null) return '$0.00';
    return '$' + value.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  }
}
