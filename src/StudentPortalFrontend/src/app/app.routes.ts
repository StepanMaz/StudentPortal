import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/auth/login/login.component';
import { RegisterPageComponent } from './pages/auth/register/register.component';
import { ClearComponent } from './pages/auth/clear/clear.component';

export const routes: Routes = [
    {
        path: 'auth',
        children: [
            {
                path: 'login',
                component: LoginPageComponent,
            },
            {
                path: 'register',
                component: RegisterPageComponent,
            },
            {
                path: 'clear',
                component: ClearComponent,
            },
        ],
    },
];
