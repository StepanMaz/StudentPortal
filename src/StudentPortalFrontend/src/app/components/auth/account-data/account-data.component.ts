import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { AuthService, User } from '../../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
    selector: 'app-account-data',
    standalone: true,
    imports: [MatCardModule, CommonModule],
    template: `
        <mat-card>
            <div *ngIf="user$ | async as data; else empty"></div>
            <ng-template #empty> </ng-template>
        </mat-card>
    `,
    styles: ``,
})
export class AccountDataComponent implements OnInit {
    user$;
    constructor(private auth: AuthService, private router: Router) {
        this.user$ = this.auth.user$;
    }
    ngOnInit(): void {
        this.user$.subscribe((x) => this.rerouteOnUnauthorized(x));
    }

    private rerouteOnUnauthorized(user: User | null) {
        if (user == null) {
            this.router.navigate(['/auth', '/login']);
        }
    }
}
