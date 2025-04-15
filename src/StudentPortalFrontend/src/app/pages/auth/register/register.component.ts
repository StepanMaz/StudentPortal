import { Component, OnInit } from '@angular/core';
import { AuthService } from '@services/auth/auth.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { combineLatest } from 'rxjs';
import { RegisterFormComponent } from '@components/auth/register/register.component';
import { User } from '@lib/user';

@Component({
    selector: 'page-register',
    standalone: true,
    imports: [RegisterFormComponent, RouterModule],
    template: `<div class="flex justify-center">
        <auth-register-form [onAuthorized]="onAuthorized" class="w-100" />
    </div>`,
    styles: ``,
})
export class RegisterPageComponent implements OnInit {
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
