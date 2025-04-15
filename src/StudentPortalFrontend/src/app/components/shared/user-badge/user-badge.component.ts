import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { User } from '@lib/user';
import { UserAvatarComponent } from '../user-avatar/user-avatar.component';

@Component({
    selector: 'app-user-badge',
    standalone: true,
    imports: [MatIconModule, MatButtonModule, UserAvatarComponent],
    template: `<div class="flex flex-row gap-4 items-center">
        <app-user-avatar [src]="user.avatarURL" [size]="48" />
        <span class="text-xl"> {{ user.firstName }} {{ user.lastName }} </span>
    </div>`,
    styles: ``,
})
export class UserBadgeComponent {
    @Input() user!: User;
}
