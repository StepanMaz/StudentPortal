import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../../services/auth/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatCommonModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { User } from '@lib/user';
import { filter } from 'rxjs';

@Component({
    selector: 'app-account-data',
    standalone: true,
    imports: [
        MatCardModule,
        MatButtonModule,
        MatInputModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatIconModule,
        MatCommonModule,
        MatSelectModule,
        CommonModule,
    ],
    template: `
        <mat-card>
            <mat-card-content *ngIf="user$ | async as data; else empty" class="p-4">
                <form [formGroup]="form" (ngSubmit)="submitForm()" class="flex flex-col items-center">
                    <mat-form-field class="w-full">
                        <mat-label>First Name</mat-label>
                        <input matInput placeholder="First Name" formControlName="firstName" />
                    </mat-form-field>

                    <mat-form-field class="w-full">
                        <mat-label>Last Name</mat-label>
                        <input matInput placeholder="Last Name" formControlName="lastName" />
                    </mat-form-field>

                    <mat-form-field class="w-full">
                        <mat-label>Email</mat-label>
                        <input matInput placeholder="Email" formControlName="email" />
                    </mat-form-field>

                    <mat-form-field>
                        <mat-select formControlName="role">
                            <mat-option *ngFor="let role; in: roles" [value]="role">{{ role }}</mat-option>
                        </mat-select>
                    </mat-form-field>

                    <button mat-raised-button color="primary" type="submit" class="max-w-40" [disabled]="form.invalid">
                        Submit
                    </button>
                </form>
            </mat-card-content>
            <mat-card-footer>
                <p class="text-center text-gray-600">
                    Already have an account?
                    <a class="text-blue-500 hover:text-blue-700 font-semibold" href="auth/login">Sign in</a>
                </p>
            </mat-card-footer>
            <ng-template #empty> </ng-template>
        </mat-card>
    `,
    styles: ``,
})
export class AccountDataComponent implements OnInit {
    form!: FormGroup;
    user$;
    constructor(private fb: FormBuilder, private auth: AuthService, private router: Router) {
        this.user$ = this.auth.user$;

        this.user$.pipe(filter((x) => x != null)).subscribe((x) => this.createForm(x));
    }

    private createForm(user: User) {
        this.form = this.fb.group({
            firstName: [user.firstName, Validators.required],
            lastName: [user.lastName, Validators.required],
            email: [user.email, [Validators.required, Validators.email]],
            role: [user.role.toString(), Validators.required],
        });
    }

    submitForm() {
        throw new Error('Method not implemented.');
    }

    ngOnInit(): void {
        this.user$.subscribe((x) => this.rerouteOnUnauthorized(x));
    }

    private rerouteOnUnauthorized(user: User | null) {
        if (user == null) {
            this.router.navigate(['auth', 'login']);
        }
    }
}
