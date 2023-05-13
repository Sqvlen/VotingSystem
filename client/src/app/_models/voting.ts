import { IName } from "./name";
import { IVote } from "./vote";

export interface IVoting {
    id: number;
    title: string;
    details: string;
    description: string;
    created: Date;
    countVote: number;
    names: IName[];
    votes: IVote[];
    isClosed: boolean;
}