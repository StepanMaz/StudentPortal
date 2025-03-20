import { PasswordValidator } from './types';

export class AllValidPasswordValidator implements PasswordValidator {
    isValid(input: string): boolean {
        return true;
    }
    getErrorList(input: string): string[] {
        return [];
    }

    static Instance = new AllValidPasswordValidator();
}
