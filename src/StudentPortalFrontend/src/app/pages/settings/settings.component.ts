import { Component } from '@angular/core';
import { PageComponent } from '../../components/shared/page/page.component';
import { AuthService } from '@services/auth/auth.service';
import { UserAvatarComponent } from '../../components/shared/user-avatar/user-avatar.component';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { User, UserRole } from '@lib/user';
import { MatSelectModule } from '@angular/material/select';

@Component({
    selector: 'page-settings',
    standalone: true,
    imports: [
        PageComponent,
        UserAvatarComponent,
        CommonModule,
        MatButtonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
    ],
    template: `
        <app-page>
            <div class="flex flex-col gap-4">
                @if (auth.user$ | async; as user) {
                    <div class="flex flex-row gap-4 items-center">
                        <app-user-avatar [src]="user.avatarURL" [size]="72" />
                        <button mat-raised-button color="primary" (click)="uploadAvatar()">Upload avatar</button>
                    </div>
                    <hr />
                    <div>
                        <form [formGroup]="formGroup" *ngIf="formGroup" (ngSubmit)="submitForm()" class="max-w-100">
                            <mat-form-field class="w-full">
                                <mat-label> First Name </mat-label>
                                <input matInput placeholder="First Name" formControlName="firstName" />
                            </mat-form-field>

                            <mat-form-field class="w-full">
                                <mat-label> Last Name </mat-label>
                                <input matInput placeholder="Last Name" formControlName="lastName" />
                            </mat-form-field>

                            <mat-form-field class="w-full">
                                <mat-label> Email</mat-label>
                                <input matInput placeholder="Email" formControlName="email" />
                            </mat-form-field>

                            <mat-form-field class="w-full">
                                <mat-label>Role</mat-label>
                                <mat-select formControlName="role">
                                    <mat-option [value]="userRole.Student">Student</mat-option>
                                    <mat-option [value]="userRole.Teacher">Teacher</mat-option>
                                </mat-select>
                            </mat-form-field>

                            <button mat-raised-button type="submit" [disabled]="!dataIsNoSame(user)">Submit</button>
                        </form>
                    </div>
                    <hr />
                    <div>
                        <button mat-raised-button color="primary" (click)="changePassword()">Change password</button>
                    </div>
                } @else {
                    User not authenticated
                }
            </div>
        </app-page>
    `,
    styles: ``,
})
export class SettingsPageComponent {
    formGroup!: FormGroup;
    userRole = UserRole;
    constructor(
        public auth: AuthService,
        formBuilder: FormBuilder,
        private matSnackBar: MatSnackBar,
    ) {
        auth.user$.subscribe((user) => {
            if (!user) return;
            console.log(user);

            this.formGroup = formBuilder.group({
                firstName: [user.firstName, Validators.required],
                lastName: [user.lastName, Validators.required],
                email: [user.email, [Validators.email, Validators.required]],
                role: [user.role, Validators.required],
            });
        });
    }

    dataIsNoSame(user: User) {
        if (this.formGroup.get('firstName')?.value != user.firstName) return true;
        if (this.formGroup.get('lastName')?.value != user.lastName) return true;
        if (this.formGroup.get('email')?.value != user.email) return true;
        return false;
    }

    submitForm() {
        this.matSnackBar.open('Currently this option is not available');
    }

    uploadAvatar() {
        this.matSnackBar.open('Currently this option is not available');
    }

    changePassword() {
        this.matSnackBar.open('Currently this option is not available');
    }
}
