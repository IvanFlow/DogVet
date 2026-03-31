import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'doseFrequency',
  standalone: true
})
export class DoseFrequencyPipe implements PipeTransform {
  private doseFrequencyMap: { [key: string]: string } = {
    'Daily': 'Diaria',
    'Every12Hours': 'Cada 12 horas',
    'Every8Hours': 'Cada 8 horas',
    'Every6Hours': 'Cada 6 horas',
    'Every4Hours': 'Cada 4 horas',
    'Weekly': 'Semanal'
  };

  transform(value: string | number): string {
    if (value === null || value === undefined) {
      return '';
    }

    const key = value.toString();
    return this.doseFrequencyMap[key] || value.toString();
  }
}