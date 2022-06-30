export interface IUser {
    username: string;
    displayName: string;
    token: string;
    roles: string[]
}

export interface UserLogin {
    password: string;
    username: string;
}