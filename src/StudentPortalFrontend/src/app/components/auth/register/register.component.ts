import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    ValidationErrors,
    ValidatorFn,
    Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCommonModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { RouterLink } from '@angular/router';
import { DefaultPasswordValidator } from '@lib/modules/passwordValidation';
import { User, UserRoleName } from '@lib/user';
import { AuthService } from '@services/auth/auth.service';

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
        RouterLink,
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

                    @if(form.get('password')?.errors?.['passwordStrength'] && form.touched) { @for(error of
                    form.get('password')!.errors!["passwordStrength"]!; track error) {
                    <small style="color: rgb(186, 26, 26);">
                        {{ error }}
                    </small>
                    } }
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

                    @if(form.errors?.['passwordMismatch'] && form.touched) {
                    <small style="color: rgb(186, 26, 26);"> Passwords do not match! </small>
                    }
                </mat-form-field>

                <mat-form-field>
                    <mat-select formControlName="role">
                        <mat-option *ngFor="let role of roles" [value]="role">{{ role }}</mat-option>
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
                <a class="text-blue-500 hover:text-blue-700 font-semibold" routerLink="/auth/login">Sign in</a>
            </p>
        </mat-card-footer>
    </mat-card>`,
    styles: ``,
})
export class RegisterFormComponent {
    roles = Object.values(UserRoleName);
    form: FormGroup;
    hidePassword: boolean = true;
    @Input() onAuthorized!: (user: User) => void;

    constructor(fb: FormBuilder, private auth: AuthService) {
        this.form = fb.group(
            {
                firstName: ['', Validators.required],
                lastName: ['', Validators.required],
                email: ['', [Validators.required, Validators.email]],
                password: ['', RegisterFormComponent.passwordValidator],
                confirmPassword: ['', [Validators.required, Validators.minLength(8)]],
                role: [UserRoleName.Student, Validators.required],
            },
            {
                validators: RegisterFormComponent.passwordEq('password', 'confirmPassword'),
            }
        );
    }

    private static passwordValidator(control: AbstractControl): ValidationErrors | null {
        const value = control.value;
        if (!value) {
            return { passwordStrength: ['Password is required'] };
        }

        if (DefaultPasswordValidator.Instance.isValid(value)) {
            return null;
        }

        return {
            passwordStrength: DefaultPasswordValidator.Instance.getErrorList(value),
        };
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
        if (this.form.invalid) return;

        const res = this.auth.register({
            firstName: this.form.get('firstName')!.value,
            lastName: this.form.get('lastName')!.value,
            email: this.form.get('email')!.value,
            password: this.form.get('password')!.value,
            role: this.form.get('role')!.value,
        });

        res.subscribe((user) => this.onAuthorized?.(user));
    }
}
