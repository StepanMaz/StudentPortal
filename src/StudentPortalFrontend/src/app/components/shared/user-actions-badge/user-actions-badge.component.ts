import { Component, Input, ViewChild } from '@angular/core';
import { UserBadgeComponent } from '../user-badge/user-badge.component';
import { User } from '@lib/user';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { Overlay, OverlayPositionBuilder, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { AccountActionsComponent } from '../account-actions/account-actions.component';

@Component({
    selector: 'app-user-actions-badge',
    standalone: true,
    imports: [UserBadgeComponent, MatButtonModule],
    template: `
        <button mat-button #menu_trigger style="height: 64px;" class="relative" (click)="openMenu()">
            <app-user-badge [user]="user" />
        </button>
    `,
    styles: ``,
})
export class UserActionsBadgeComponent {
    @Input() user!: User;
    @ViewChild('menu_trigger') menuTrigger!: MatButton;
    private overlayRef!: OverlayRef;

    constructor(
        private overlay: Overlay,
        private positionBuilder: OverlayPositionBuilder,
    ) {}

    openMenu() {
        if (this.overlayRef && this.overlayRef.hasAttached()) {
            this.overlayRef.detach();

            return;
        }

        const positionStrategy = this.positionBuilder
            .flexibleConnectedTo(this.menuTrigger._elementRef)
            .withPositions([
                {
                    originX: 'end',
                    originY: 'bottom',
                    overlayX: 'end',
                    overlayY: 'top',
                    offsetY: 8,
                },
            ])
            .withPush(true);

        this.overlayRef = this.overlay.create({ positionStrategy, hasBackdrop: true, backdropClass: '' });

        const menuPortal = new ComponentPortal(AccountActionsComponent);
        this.overlayRef.attach(menuPortal);

        this.overlayRef.backdropClick().subscribe(() => this.overlayRef.detach());
    }
}
