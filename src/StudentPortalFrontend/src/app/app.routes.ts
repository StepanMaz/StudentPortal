import { Routes } from '@angular/router';
import { LoginPageComponent } from './pages/auth/login/login.component';
import { RegisterPageComponent } from './pages/auth/register/register.component';
import { RootPageComponent } from './pages/root/root.component';
import { FAQPageComponent } from './pages/faq/faq.component';
import { TestResultsPageComponent } from './pages/test-results/test-results.component';
import { SettingsPageComponent } from './pages/settings/settings.component';
import { ResultsPage } from './pages/results/results.component';
import { GradingComponent } from './pages/grading/grading.component';
import { MyTestsComponent } from './pages/my-tests/my-tests.component';

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
        path: 'my-tests',
        component: MyTestsComponent,
    },
    {
        path: 'tests',
        children: [
            {
                path: '',
                component: TestResultsPageComponent,
            },
            {
                path: 'grading',
                children: [
                    {
                        path: ':id',
                        component: GradingComponent,
                    },
                ],
            },
        ],
    },
    {
        path: 'settings',
        component: SettingsPageComponent,
    },
    {
        path: 'results',
        children: [
            {
                path: ':id',
                component: ResultsPage,
            },
        ],
    },
];
