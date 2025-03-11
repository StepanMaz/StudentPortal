import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCommonModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

@Component({
    selector: 'auth-register-form',
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
    template: ` <mat-card>
        <mat-card-header class="mb-2 text-center"><span class="text-xl">Login</span></mat-card-header>
        <mat-card-content class="p-4">
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

                <mat-form-field class="w-full">
                    <mat-label>Confirm Password </mat-label>
                    <input
                        matInput
                        placeholder="Confirm Password"
                        formControlName="confirmPassword"
                        type="{{ hidePassword ? 'password' : 'text' }}"
                    />
                    <button mat-icon-button matSuffix (click)="hidePassword = !hidePassword" type="button">
                        <mat-icon>{{ hidePassword ? 'visibility_off' : 'visibility' }}</mat-icon>
                    </button>
                    <div *ngIf="form.errors?.['passwordMismatch'] && form.touched">Passwords do not match!</div>
                </mat-form-field>

                <button mat-raised-button color="primary" type="submit" class="max-w-40">Submit</button>
            </form>
        </mat-card-content>
        <mat-card-footer>
            <p class="text-center text-gray-600">
                Don't have an account?
                <a class="text-blue-500 hover:text-blue-700 font-semibold" href="auth/register">Sign up</a>
            </p>
        </mat-card-footer>
    </mat-card>`,
    styles: ``,
})
export class RegisterFormComponent {
    form: FormGroup;
    hidePassword: boolean = true;

    constructor(fb: FormBuilder) {
        this.form = fb.group(
            {
                firstName: ['', Validators.required],
                lastName: ['', Validators.required],
                email: ['', [Validators.required, Validators.email]],
                password: ['', [Validators.required, Validators.minLength(6)]],
                confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
            },
            {
                validators: RegisterFormComponent.passwordEq('password', 'confirmPassword'),
            }
        );
    }

    private static passwordEq =
        (passwordFieldName: string, confirmFieldName: string): ValidatorFn =>
        (control) => {
            const password1 = control.get(passwordFieldName)?.value;
            const password2 = control.get(confirmFieldName)?.value;

            if (password1 != password2) {
                return { passwordMismatch: true };
            }

            return null;
        };

    submitForm() {
        throw new Error('Method not implemented.');
    }
}
