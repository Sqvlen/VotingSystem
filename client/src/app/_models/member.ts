import { IName } from "./name";
import { IVote } from "./vote";
import { IVoting } from "./voting";

export interface IMember {
    id: number;
    name: IName;
    votes: IVote[];
    votings: IVoting[];
}