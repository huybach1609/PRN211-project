export interface IUser {
    id: number;
    userName?: string;
    fullName?: string;
    email?: string;
    role?: number;
}
export class User implements IUser {
    constructor(public id: number,
        public userName?: string,
        public fullName?: string,
        public email?: string,
        public role?: number,
    ) {
    }
}
export interface AuthResponse {
    result: boolean;
    message: string;
    accessToken: string;
}