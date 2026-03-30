import { Pipe, PipeTransform, Inject, LOCALE_ID } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({
  name: 'spanishDate',
  standalone: true
})
export class SpanishDatePipe implements PipeTransform {
  constructor(@Inject(LOCALE_ID) private locale: string) {}

  transform(value: string | Date | null | undefined, format: string = 'mediumDate'): string {
    if (!value) return '';
    
    const datePipe = new DatePipe(this.locale);
    
    const formatMap: { [key: string]: string } = {
      'mediumDate': 'dd MMM yyyy',
      'fullDate': 'EEEE, d \'de\' MMMM \'de\' yyyy'
    };

    const dateFormat = formatMap[format] || format;
    return datePipe.transform(value, dateFormat) || '';
  }
}
