import { Component } from '@angular/core';

@Component({
    selector: 'app-logo',
    standalone: true,
    imports: [],
    template: `
        <div class="rounded-full overflow-hidden max-w-64">
            <img src="icons/logo.svg" />
        </div>
    `,
    styles: ``,
})
export class LogoComponent {}
