import { Component } from '@angular/core';

@Component({
    selector: 'app-header-logo',
    standalone: true,
    imports: [],
    template: `
        <div class="rounded-xs overflow-hidden">
            <img class="logo-image" src="icons/logo.svg" />
        </div>
    `,
    styles: `
        .logo-image {
            width: 270px;
            height: 60px;
            min-width: 270px;
        }
    `,
})
export class LogoComponent {}
