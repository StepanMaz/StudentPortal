import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@lib/user';
import { map } from 'rxjs';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    constructor(private httpClient: HttpClient) {}

    getUser(id: string) {
        return this.httpClient
            .get<SimpleUser>(`/api/auth/users/${id}`)
            .pipe(map((x) => User.fromJSON({ ...x, role: x.roles[0], avatarURL: null })));
    }
}

export type SimpleUser = {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    roles: string[];
};
