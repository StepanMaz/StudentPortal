import { UserRoleName } from '@lib/user';

export type RegisterData = {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    role: UserRoleName;
};

export type LoginData = {
    email: string;
    password: string;
};

export type AuthResult = {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    roles: UserRoleName[];
    jwtToken: string;
};
