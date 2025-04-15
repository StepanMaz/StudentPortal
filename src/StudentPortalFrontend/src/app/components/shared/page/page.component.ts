import { Component } from '@angular/core';

@Component({
    selector: 'app-page',
    standalone: true,
    imports: [],
    template: `
        <div class="page p-4">
            <ng-content />
        </div>
    `,
    styles: ``,
})
export class PageComponent {}
