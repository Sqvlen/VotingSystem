import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { IVote } from '../_models/vote';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPaginationVote, PaginationVote } from '../_models/paginationVote';
import { VoteParams } from '../_models/voteParams';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VoteService {
  private baseUrl = environment.apiUrl;

  votes: IVote[] = [];
  pagination = new PaginationVote();
  voteParams = new VoteParams();

  constructor(private http: HttpClient) { }

  vote(username: string, votingId: number, nameId: number) {
    return this.http.post(this.baseUrl + 'votes/' + username + '/' + votingId + '/' + nameId, {});
  }

  getVotes() {
    if (this.votes.length > 0) {
      const pagesReceived = Math.ceil(this.votes.length / this.voteParams.pageSize);

      if (this.voteParams.pageNumber <= pagesReceived) {
        this.pagination.data =
          this.votes.slice((this.voteParams.pageNumber - 1) * this.voteParams.pageSize,
            this.voteParams.pageNumber * this.voteParams.pageSize);

        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.voteParams.userId !== 0) {
      params = params.append('userId', this.voteParams.userId.toString());
    }

    if (this.voteParams.votingId !== 0) {
      params = params.append('votinId', this.voteParams.votingId.toString());
    }

    params = params.append('pageNumber', this.voteParams.pageNumber.toString());
    params = params.append('pageSize', this.voteParams.pageSize.toString());

    return this.http.get<IPaginationVote>(this.baseUrl + 'votes', { observe: 'response', params })
      .pipe(
        map(response => {
          this.votes = [...this.votes, ...response.body!.data];
          this.pagination = response.body!;
          return this.pagination;
        })
      );
  }

  getVotesById(userId: number) {
    this.voteParams.userId = userId;
    return this.getVotes();
  }

  getCountVotesByVotingId(votingId: number) {
    return this.http.get<IVote[]>(this.baseUrl + 'votes/count/' + votingId);
  }

  setVoteParams(params: VoteParams) {
    this.voteParams = params;
  }

  getVoteParams() {
    return this.voteParams;
  }
}
