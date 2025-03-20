import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '@services/auth/auth.service';

@Component({
    selector: 'app-clear',
    standalone: true,
    imports: [RouterModule],
    template: ` <p>Removes account data</p> `,
    styles: ``,
})
export class ClearComponent implements OnInit {
    constructor(private auth: AuthService, private router: Router) {}
    ngOnInit(): void {
        this.auth.clearUser();
        this.router.navigate(['/']);
    }
}
