import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-grading-button',
    standalone: true,
    imports: [CommonModule],
    template: `
        <div
            class="rounded-full cursor-pointer flex items-center w-fit h-9"
            [style.background-color]="Color"
            (click)="click.emit()"
        >
            @if (InfoText) {
                <div class="rounded-full bg-gray-200 p-2">{{ InfoText }}</div>
            }
            <div class="text-white mx-2">{{ ButtonText }}</div>
        </div>
    `,
    styles: ``,
})
export class GradingButtonComponent {
    @Input() InfoText: string | undefined;
    @Input() ButtonText!: string;
    @Input() Color!: string;
    @Output() click: EventEmitter<any> = new EventEmitter();
}
