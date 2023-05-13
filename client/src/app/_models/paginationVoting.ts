import { IVoting } from "./voting";

export interface IPaginationVoting {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IVoting[];
}

export class PaginationVoting implements IPaginationVoting {
    pageIndex: number = 1;
    pageSize: number = 5;
    count: number = 1;
    data: IVoting[] = [];
}