import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/auth/login/login.component';

export const routes: Routes = [
    {
        path: 'auth',
        children: [
            {
                path: 'login',
                component: LoginPageComponent,
            },
        ],
    },
];
