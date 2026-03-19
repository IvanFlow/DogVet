import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  standalone: true,
  template: `
    <div class="container">
      <h1>{{ title }}</h1>
    </div>
  `,
  styles: [`
    .container {
      display: flex;
      justify-content: center;
      align-items: center;
      height: 100vh;
      font-family: Arial, sans-serif;
    }
    h1 {
      font-size: 3em;
      color: #333;
      margin: 0;
    }
  `]
})
export class AppComponent {
  title = 'Hello World!';
}
