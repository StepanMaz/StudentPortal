import { Component, Input } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'landing-info-card',
    standalone: true,
    imports: [MatIcon],
    template: `
        <div
            class="border border-4 text-center rounded-xl flex items-center justify-center flex-col p-4 info-box"
            style="border-color: {{ border_color }};"
        >
            <mat-icon class="info-box-icon" fontSet="material-icons-outlined">{{ icon }}</mat-icon>
            <p class="text-xl">{{ text }}</p>
        </div>
    `,
    styles: `
    .info-box {
      width: 290px;
      height: 320px;
    }
    .info-box-icon {
      width: 64px;
      height: 80px;
      font-size: 64px; 
    }`,
})
export class InfoCardComponent {
    @Input() icon!: string;
    @Input() text!: string;
    @Input() border_color!: string;
}
