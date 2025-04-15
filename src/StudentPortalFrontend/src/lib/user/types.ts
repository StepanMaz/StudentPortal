export enum UserRoleName {
    Student = 'Student',
    Teacher = 'Teacher',
}

enum ExtendedUserRoleName {
    Admin = 'Admin',
}

export type AllRoleNames = UserRoleName | ExtendedUserRoleName;
export const AllRoleNames = { ...UserRoleName, ...ExtendedUserRoleName };

export type UserData = {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    role: string | null;
    avatarURL: string | null;
};
