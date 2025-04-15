import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatCommonModule } from '@angular/material/core';
import { AuthService } from '@services/auth/auth.service';
import { RouterLink } from '@angular/router';
import { User } from '@lib/user';

@Component({
    selector: 'auth-login-form',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        MatButtonModule,
        MatFormFieldModule,
        MatCardModule,
        MatIconModule,
        MatInputModule,
        MatCommonModule,
        RouterLink,
    ],
    template: `
        <mat-card>
            <mat-card-header class="mb-2 text-center"><span class="text-xl">Login</span></mat-card-header>
            <mat-card-content class="p-4">
                <form [formGroup]="form" (ngSubmit)="submitForm()" class="flex flex-col items-center">
                    <mat-form-field class="w-full">
                        <mat-label>Email</mat-label>
                        <input matInput placeholder="Email" formControlName="email" />
                    </mat-form-field>

                    <mat-form-field class="w-full">
                        <mat-label>Password</mat-label>
                        <input
                            matInput
                            placeholder="Password"
                            formControlName="password"
                            type="{{ hidePassword ? 'password' : 'text' }}"
                        />
                        <button mat-icon-button matSuffix (click)="hidePassword = !hidePassword" type="button">
                            <mat-icon>{{ hidePassword ? 'visibility_off' : 'visibility' }}</mat-icon>
                        </button>
                    </mat-form-field>

                    <button mat-raised-button color="primary" type="submit" class="max-w-40" [disabled]="form.invalid">
                        Submit
                    </button>

                    @if (message) {
                        <small class="text-red-500 p-4">{{ message }} </small>
                    }
                </form>
            </mat-card-content>
            <mat-card-footer>
                <p class="text-center text-gray-600">
                    Don't have an account?
                    <a class="text-blue-500 hover:text-blue-700 font-semibold" routerLink="/auth/register">Sign up</a>
                </p>
            </mat-card-footer>
        </mat-card>
    `,
    styles: ``,
})
export class LoginFormComponent {
    form;
    hidePassword: boolean = true;
    message: string | null = null;

    @Input()
    onAuthorized!: (user: User) => void;

    constructor(
        fb: FormBuilder,
        private auth: AuthService,
    ) {
        this.form = fb.group({
            email: ['', [Validators.required]],
            password: ['', [Validators.required]],
        });
    }

    submitForm() {
        if (this.form.invalid) return;

        const credentials = {
            email: this.form.get('email')?.value!,
            password: this.form.get('password')?.value!,
        };

        const res = this.auth.login(credentials);

        res.subscribe(
            (u) => this.onAuthorized(u),
            () => (this.message = 'Invalid email or password'),
        );
    }
}
