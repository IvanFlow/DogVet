import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'species',
  standalone: true
})
export class SpeciesPipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) return '—';

    const speciesMap: { [key: string]: string } = {
      'Dog': 'Perro',
      'Cat': 'Gato',
      'Bird': 'Ave',
      'Rabbit': 'Conejo',
      'Hamster': 'Hámster',
      'Other': 'Otro'
    };

    return speciesMap[value] || value;
  }
}
