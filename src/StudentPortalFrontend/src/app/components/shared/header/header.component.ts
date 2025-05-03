import { Component } from '@angular/core';
import { LogoComponent } from './logo/logo.component';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { AuthService } from '@services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { UserRole } from '@lib/user';
import { UserActionsBadgeComponent } from '../user-actions-badge/user-actions-badge.component';

@Component({
    selector: 'app-header',
    standalone: true,
    imports: [LogoComponent, MatButtonModule, RouterModule, CommonModule, UserActionsBadgeComponent],
    template: `
        <div class="p-3 flex flex-row justify-between">
            <div class="flex flex-row gap-4 text-xl items-center">
                <a routerLink="/">
                    <app-header-logo />
                </a>

                @if (auth.user$ | async; as user) {
                    @if (user.role == UserRole.Teacher) {
                        <a href="/pages/editor/files" class="hover:underline">Materials</a>
                        <a routerLink="/tests" class="hover:underline">Tests</a>
                    }
                }

                <a routerLink="/faq" class="hover:underline"> FAQ </a>
            </div>

            <div class="flex flex-row gap-4">
                @if (auth.user$ | async; as user) {
                    <app-user-actions-badge [user]="user" />
                } @else {
                    <a mat-button routerLink="/auth/login">Log In</a>
                    <a mat-raised-button color="primary" routerLink="/auth/register">Join Now</a>
                }
            </div>
        </div>
    `,
    styles: ``,
})
export class HeaderComponent {
    UserRole = UserRole;
    constructor(public auth: AuthService) {}
}
