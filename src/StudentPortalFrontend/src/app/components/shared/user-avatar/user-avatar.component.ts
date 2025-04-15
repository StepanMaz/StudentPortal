import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'app-user-avatar',
    standalone: true,
    imports: [],
    template: `
        <div class="rounded-full vertical-center overflow-hidden" [style.width.px]="size" [style.height.px]="size">
            <img src="{{ src ?? '/images/default_profile.png' }}" [style.width.px]="size" [style.height.px]="size" />
        </div>
    `,
    styles: ``,
})
export class UserAvatarComponent {
    @Input() src: string | null | undefined;
    @Input() size!: number;
}
