import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'age',
  standalone: true
})
export class AgePipe implements PipeTransform {
  transform(dateOfBirth: string | Date | null | undefined): string {
    if (!dateOfBirth) return '—';

    const birth = new Date(dateOfBirth);
    const today = new Date();

    const months =
      (today.getFullYear() - birth.getFullYear()) * 12 +
      (today.getMonth() - birth.getMonth()) -
      (today.getDate() < birth.getDate() ? 1 : 0);

    if (months < 12) {
      return months <= 1 ? `${months} mes` : `${months} meses`;
    }

    const years = Math.floor(months / 12);
    return years === 1 ? `${years} año` : `${years} años`;
  }
}
