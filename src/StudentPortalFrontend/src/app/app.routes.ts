import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/auth/login/login.component';
import { RegisterPageComponent } from './pages/auth/register/register.component';
import { RootPageComponent } from './pages/root/root.component';
import { FAQPageComponent } from './pages/faq/faq.component';
import { TestResultsPageComponent } from './pages/test-results/test-results.component';
import { SettingsPageComponent } from './pages/settings/settings.component';

export const routes: Routes = [
    {
        path: '',
        component: RootPageComponent,
    },
    {
        path: 'test',
        component: LoginPageComponent,
    },
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
        ],
    },
    {
        path: 'faq',
        component: FAQPageComponent,
    },
    {
        path: 'tests',
        component: TestResultsPageComponent,
    },
    {
        path: 'settings',
        component: SettingsPageComponent,
    },
];
