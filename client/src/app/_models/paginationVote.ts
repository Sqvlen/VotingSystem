import { IVote } from "./vote";

export interface IPaginationVote {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IVote[];
}

export class PaginationVote implements IPaginationVote {
    pageIndex: number = 1;
    pageSize: number = 5;
    count: number = 1;
    data: IVote[] = [];
}