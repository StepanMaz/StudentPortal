import { AllRoleNames } from './types';

export class UserRole {
    private constructor(private roleName: AllRoleNames | null = null) {}

    public static Student = new UserRole(AllRoleNames.Student);
    public static Teacher = new UserRole(AllRoleNames.Teacher);
    public static None = new UserRole(null);

    static create(roleName: string | null): UserRole | (typeof UserRole)['None'] {
        if (roleName && roleName in AllRoleNames) {
            switch (roleName) {
                case AllRoleNames.Student:
                    return UserRole.Student;
                case AllRoleNames.Teacher:
                    return UserRole.Teacher;
            }

            console.warn('[warn] User role could not be determined.');

            return UserRole.None;
        }

        return UserRole.None;
    }

    public isNone() {
        return this == UserRole.None;
    }

    public isTeacher() {
        return this == UserRole.Teacher;
    }

    public isStudent() {
        return this == UserRole.Student;
    }

    toJSON() {
        return this.roleName;
    }

    toString(): AllRoleNames | 'None' {
        return this.roleName ?? 'None';
    }
}
