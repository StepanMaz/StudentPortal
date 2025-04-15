import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './components/shared/header/header.component';
import { FooterComponent } from './components/shared/footer/footer.component';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [RouterModule, HeaderComponent, FooterComponent],
    template: `
        <app-header />
        <router-outlet />
        <app-footer />
    `,
    styles: ``,
})
export class AppComponent {
    title = 'StudentPortalFrontend';
}
