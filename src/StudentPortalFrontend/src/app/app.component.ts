import { Component } from '@angular/core';
import { LogoComponent } from './logo/logo.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [LogoComponent],
  template: `<div>
    <a href="/" class="noninteractive">
      <app-logo />
    </a>
  </div>`,
  styles: `
    .noninteractive {
      text-decoration: none;
      user-select: none;
      color: inherit;
      pointer-events: none;
    }
  `,
})
export class AppComponent {
  title = 'StudentPortalFrontend';
}
