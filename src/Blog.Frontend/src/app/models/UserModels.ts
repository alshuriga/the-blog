export type User = {
    username: string;
    password: string;
}

export type UserClaims = {
    username: string;
    role: string[] | string;
}

export type UserListDTO = {
    id: string;
    username: string;
    email: string;
    roles: string[];
}

export type UsersListVM = {
    normalUsers: UserListDTO[];
    adminUsers: UserListDTO[];
}

export type UserSignUpDTO = {
    username: string;
    email: string;
    password: string;
    repeatPassword: string;
}