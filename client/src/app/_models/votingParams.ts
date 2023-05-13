export class VotingParams {
    userId: number = 0;
    votingId: number = 0;
    sortByDate?: string;
    sortByVotes?: string;
    sortByAlphabetical?: string;
    pageNumber: number = 1;
    pageSize: number = 12;
    search?: string;
}