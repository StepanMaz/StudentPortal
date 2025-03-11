import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { LoginDTO, RegisterDTO, AuthResult } from './types';

interface IAuthService {
    login(credentials: LoginDTO): Observable<User>;
    register(credentials: RegisterDTO): Observable<User>;
}

@Injectable({
    providedIn: 'root',
})
export class AuthService implements IAuthService {
    private _user: BehaviorSubject<User | null>;
    user$: Observable<User | null>;

    constructor(private http: HttpClient) {
        this._user = new BehaviorSubject(this.getUser());
        this.user$ = this._user.asObservable();

        this._user.subscribe((user) => {
            if (user) this.saveUser(user);
        });
    }

    login(credentials: LoginDTO): Observable<User> {
        const payload = {
            email: credentials.email,
            username: credentials.email,
            password: credentials.password,
        };

        const res = this.http.post<AuthResult>('/api/auth/login', payload);

        const user = res.pipe(map(AuthService.createUser));

        user.subscribe((u) => this.updateUser(u));

        return user;
    }

    register(credentials: RegisterDTO): Observable<User> {
        const payload = {
            email: credentials.email,
            username: credentials.email,
            password: credentials.password,
            firstName: credentials.firstName,
            lastName: credentials.lastName,
            role: credentials.role,
        };

        const res = this.http.post<AuthResult>('/api/auth/register', payload);

        const user = res.pipe(map(AuthService.createUser));

        user.subscribe((u) => this.updateUser(u));

        return user;
    }

    clearUser() {
        localStorage.removeItem('local-user');
        this._user.next(null);
    }

    private updateUser(user: User) {
        this._user.next(user);
    }

    private static createUser(res: AuthResult) {
        return new User(res.id, res.firstName, res.lastName, res.email);
    }

    private saveUser(user: User) {
        localStorage.setItem('local-user', JSON.stringify(user));
    }

    private getUser(): User | null {
        const res = localStorage.getItem('local-user');

        if (!res) return null;

        const { id, email, firstName, lastName } = JSON.parse(res);

        return new User(id, firstName, lastName, email);
    }
}

export class User {
    constructor(readonly id: string, readonly firstName: string, readonly lastName: string, readonly email: string) {}

    toJSON() {
        return {
            id: this.id,
            firstName: this.firstName,
            lastName: this.lastName,
            email: this.email,
        };
    }
}
