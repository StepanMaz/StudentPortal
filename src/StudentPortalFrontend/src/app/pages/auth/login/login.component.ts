import { Component, OnInit } from '@angular/core';
import { LoginFormComponent } from '@components/auth/login/login.component';
import { AuthService, User } from '@services/auth/auth.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { combineLatest } from 'rxjs';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [LoginFormComponent, RouterModule],
    template: `<div class="flex justify-center">
        <auth-login-form [onAuthorized]="onAuthorized" class="w-100" />
    </div>`,
    styles: ``,
})
export class LoginPageComponent implements OnInit {
    constructor(private auth: AuthService, private router: Router, private route: ActivatedRoute) {}

    ngOnInit(): void {
        combineLatest([this.route.queryParams, this.auth.user$]).subscribe(([params, user]) => {
            this.rerouteOnAuthorized(user, params['nextRoute']);
        });
    }

    onAuthorized(user: User) {
        this.route.queryParams.subscribe((params) => this.rerouteOnAuthorized(user, params['nextRoute']));
    }

    private rerouteOnAuthorized(user: User | null, route = ['/']) {
        if (user) {
            this.router.navigate(route);
        }
    }
}
