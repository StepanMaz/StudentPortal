import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './components/shared/header/header.component';
import { FooterComponent } from './components/shared/footer/footer.component';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [RouterModule, HeaderComponent, FooterComponent],
    template: `
        <div class="flex flex-col min-h-screen">
            <app-header />
            <div class="flex-auto grid grid-rows-[auto_1fr_auto]">
                <router-outlet />
            </div>
            <app-footer />
        </div>
    `,
    styles: ``,
})
export class AppComponent {
    title = 'StudentPortalFrontend';
}
