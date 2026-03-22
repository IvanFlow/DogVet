import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'gender',
  standalone: true
})
export class GenderPipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) return '';
    
    const genderMap: { [key: string]: string } = {
      'Male': 'Macho',
      'Female': 'Hembra'
    };

    return genderMap[value] || value;
  }
}
