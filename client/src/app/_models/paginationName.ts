import { IName } from "./name";

export interface IPaginationName {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IName[];
}

export class PaginationName implements IPaginationName {
    pageIndex: number = 1;
    pageSize: number = 50;
    count: number = 1;
    data: IName[] = [];
}