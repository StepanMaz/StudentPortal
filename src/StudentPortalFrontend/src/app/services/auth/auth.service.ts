import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject, map, Observable, shareReplay } from 'rxjs';
import { LoginData, RegisterData, AuthResult } from './types';
import { isPlatformBrowser } from '@angular/common';
import { User, UserRole } from '@lib/user';

interface IAuthService {
    login(credentials: LoginData): Observable<User>;
    register(credentials: RegisterData): Observable<User>;
}

@Injectable({
    providedIn: 'root',
})
export class AuthService implements IAuthService {
    private _user: BehaviorSubject<User | null>;
    user$: Observable<User | null>;

    constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) {
        this._user = new BehaviorSubject(this.getUser());
        this.user$ = this._user.asObservable();

        this._user.subscribe((user) => {
            if (user) this.saveUser(user);
        });
    }

    login(credentials: LoginData): Observable<User> {
        const payload = {
            email: credentials.email,
            username: credentials.email,
            password: credentials.password,
        };

        const res = this.http.post<AuthResult>('/api/auth/login', payload).pipe(shareReplay(1));

        const user = res.pipe(map((x) => this.parseResponse(x).user));

        user.subscribe((u) => this.updateUser(u));

        return user;
    }

    register(credentials: RegisterData): Observable<User> {
        const payload = {
            email: credentials.email,
            username: credentials.email,
            password: credentials.password,
            firstName: credentials.firstName,
            lastName: credentials.lastName,
            role: credentials.role,
        };

        const res = this.http.post<AuthResult>('/api/auth/register', payload).pipe(shareReplay(1));

        const user = res.pipe(map((x) => this.parseResponse(x).user));

        user.subscribe((u) => this.updateUser(u));

        return null!;
    }

    private parseResponse(res: AuthResult) {
        const { jwtToken, roles, ...rest } = res;

        return {
            token: jwtToken,
            user: User.fromJSON({ role: roles[0], ...rest }),
        };
    }

    clearUser() {
        localStorage.removeItem('local-user');
        this._user.next(null);
    }

    private updateUser(user: User) {
        this._user.next(user);
    }

    private saveUser(user: User) {
        localStorage.setItem('local-user', JSON.stringify(user));
    }

    private getUser(): User | null {
        if (!isPlatformBrowser(this.platformId)) return null;

        const res = localStorage.getItem('local-user');

        if (!res) return null;

        return User.fromJSON(JSON.parse(res));
    }
}
