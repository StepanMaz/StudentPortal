import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LogoComponent } from './components/shared/logo/logo.component';
import { HttpClientModule } from '@angular/common/http';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [LogoComponent, RouterModule],
    template: `<div>
        <a href="/" class="noninteractive">
            <app-logo />
        </a>
        <router-outlet></router-outlet>
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
