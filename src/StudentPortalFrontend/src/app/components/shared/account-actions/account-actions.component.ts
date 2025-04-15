import { Component, Input } from '@angular/core';
import { User } from '@lib/user';
import { UserAvatarComponent } from '../user-avatar/user-avatar.component';
import { RouterModule } from '@angular/router';
import { AuthService } from '@services/auth/auth.service';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-account-actions',
    standalone: true,
    imports: [UserAvatarComponent, RouterModule, CommonModule],
    template: `
        <div class="flex flex-col container rounded-sm min-w-64">
            @if (auth.user$ | async; as user) {
                <div class="flex flex-row gap-4 p-3">
                    <div>
                        <app-user-avatar [src]="user.avatarURL" [size]="48" />
                    </div>
                    <div class="flex flex-col">
                        <span>{{ user.firstName }} {{ user.lastName }}</span>
                        <span>{{ user.email }}</span>
                    </div>
                </div>
                <a class="action_button" routerLink="/materials"> My Materials </a>
                <a class="action_button" routerLink="/tests"> My Tests </a>
                <a class="action_button" routerLink="/settings"> Settings </a>
                <button class="action_button" (click)="signOut()">Sign out</button>
            } @else {
                <a class="action_button" routerLink="/auth/login">Log In</a>
                <a class="action_button" routerLink="/auth/register">Sing Up</a>
            }
        </div>
    `,
    styles: `
        .container {
            background-color: #f4f3f6;
            overflow: hidden;
        }

        .action_button {
            width: 100%;
            line-height: 40px;
            padding: 0 12px;
            text-align: left;
        }

        .action_button:not(:first-child) {
            border-top: 1px solid #ccc;
        }

        .action_button:hover {
            background-color: rgb(224, 223, 226);
            transition: background-color 0.2s;
        }
    `,
})
export class AccountActionsComponent {
    constructor(public auth: AuthService) {}

    signOut() {
        this.auth.clearUser();
    }
}
