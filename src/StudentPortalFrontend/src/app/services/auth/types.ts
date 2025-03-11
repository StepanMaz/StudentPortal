export type RegisterDTO = {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    role: 'Teacher' | 'Student';
};

export type LoginDTO = {
    email: string;
    password: string;
};

export type AuthResult = {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    jwtToken: string;
};
