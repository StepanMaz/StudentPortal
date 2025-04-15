import { PasswordValidator } from './types';

const minLength = 8;

function isValidLength(password: string): boolean {
    return password.length >= minLength;
}

function containsUpperCaseLetter(password: string): boolean {
    for (const char of password) {
        const upperVariant = char.toUpperCase();
        const lowerVariant = char.toLowerCase();

        if (upperVariant == lowerVariant) continue; // skip if char does not have uppercase variant

        if (char == upperVariant) return true;
    }
    return false;
}

function containsLowerCaseLetter(password: string): boolean {
    for (const char of password) {
        const upperVariant = char.toUpperCase();
        const lowerVariant = char.toLowerCase();

        if (upperVariant == lowerVariant) continue; // skip if char does not have uppercase variant

        if (char == lowerVariant) return true;
    }
    return false;
}

function containsDigit(password: string): boolean {
    return /\d/.test(password);
}

function hasNonAlphanumeric(password: string): boolean {
    return /[^a-zA-Z-1-9]/.test(password);
}

function isValidPassword(password: string): boolean {
    return (
        isValidLength(password) &&
        containsUpperCaseLetter(password) &&
        containsLowerCaseLetter(password) &&
        containsDigit(password) &&
        hasNonAlphanumeric(password)
    );
}

function getValidationErrors(password: string): string[] {
    const errors: string[] = [];

    if (!isValidPassword(password)) {
        errors.push(
            `Password must contain at least ${minLength} characters, symbol, including uppercase, lowercase, and a number.`,
        );
    }

    return errors;
}

export class DefaultPasswordValidator implements PasswordValidator {
    isValid(input: string): boolean {
        return isValidPassword(input);
    }
    getErrorList(input: string): string[] {
        return getValidationErrors(input);
    }

    static Instance = new DefaultPasswordValidator();
}
