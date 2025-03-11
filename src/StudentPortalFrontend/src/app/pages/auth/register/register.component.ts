import { Component, OnInit } from '@angular/core';
import { AuthService, User } from '@services/auth/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { combineLatest } from 'rxjs';
import { RegisterFormComponent } from '@components/auth/register/register.component';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [RegisterFormComponent],
    template: `<div class="flex justify-center">
        <auth-register-form class="w-100" />
    </div>`,
    styles: ``,
})
export class RegisterComponent implements OnInit {
    constructor(private auth: AuthService, private router: Router, private route: ActivatedRoute) {}
    ngOnInit(): void {
        combineLatest([this.route.queryParams, this.auth.user$]).subscribe(([params, user]) => {
            this.rerouteOnAuthorized(user, params['nextRoute']);
        });
    }

    private rerouteOnAuthorized(user: User | null, route = ['/']) {
        if (user) {
            this.router.navigate(route);
        }
    }
}
