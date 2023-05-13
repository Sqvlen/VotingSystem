import { IMember } from "./member";
import { IUser } from "./user";

export interface IPaginationUser {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMember[];
}

export class PaginationUser implements IPaginationUser {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IMember[] = [];
}