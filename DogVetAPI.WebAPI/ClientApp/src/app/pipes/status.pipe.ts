import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'status',
  standalone: true
})
export class StatusPipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) return '';
    
    const statusMap: { [key: string]: string } = {
      'Completed': 'Completado',
      'Pending': 'Pendiente',
      'Paid': 'Pagado',
      'Follow-up': 'Seguimiento',
      'Prescribed': 'Prescrito',
      'Administered': 'Administrado'
    };

    return statusMap[value] || value;
  }
}
