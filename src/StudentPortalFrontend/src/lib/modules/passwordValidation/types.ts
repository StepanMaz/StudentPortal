export interface PasswordValidator {
    isValid(input: string): boolean;
    getErrorList(input: string): string[];
}
