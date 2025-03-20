import { UserRole } from './role';
import { UserData } from './types';

export class User {
    constructor(
        readonly id: string,
        readonly firstName: string,
        readonly lastName: string,
        readonly email: string,
        readonly role: UserRole
    ) {}

    toJSON(): UserData {
        return {
            id: this.id,
            firstName: this.firstName,
            lastName: this.lastName,
            email: this.email,
            role: this.role.toJSON(),
        };
    }

    static fromJSON(obj: UserData) {
        return new User(obj.id, obj.firstName, obj.lastName, obj.email, UserRole.create(obj.role));
    }
}
