import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IVoting } from '../_models/voting';
import { environment } from 'src/environments/environment.development';
import { IUser } from '../_models/user';
import { AccountService } from './account.service';
import { map, of, take } from 'rxjs';
import { ICreateVoting } from '../_models/createVoting';
import { IUpdateVoting } from '../_models/updateVoting';
import { IName } from '../_models/name';
import { IPaginationVoting, PaginationVoting } from '../_models/paginationVoting';
import { VotingParams } from '../_models/votingParams';

@Injectable({
  providedIn: 'root'
})
export class VotingService {
  private apiUrl = environment.apiUrl;
  private baseUrl = environment.apiUrl + 'votings/';

  votings: IVoting[] = [];
  pagination = new PaginationVoting();
  votingParams = new VotingParams();

  ownPagination = new PaginationVoting();
  ownVotings: IVoting[] = [];
  votingByUernameParams = new VotingParams();

  constructor(private http: HttpClient) {
  }

  getVotings(useCache: boolean) {
    if (useCache === false) {
      this.votings = [];
    }

    if (this.votings.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.votings.length / this.votingParams.pageSize);

      if (this.votingParams.pageNumber <= pagesReceived) {
        this.pagination.data =
          this.votings.slice((this.votingParams.pageNumber - 1) * this.votingParams.pageSize,
            this.votingParams.pageNumber * this.votingParams.pageSize);

        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.votingParams.userId !== 0) {
      params = params.append('userId', this.votingParams.userId.toString());
    }

    if (this.votingParams.votingId !== 0) {
      params = params.append('votinId', this.votingParams.votingId.toString());
    }

    if (this.votingParams.search) {
      params = params.append('search', this.votingParams.search);
    }

    if (this.votingParams.sortByAlphabetical) {
      params = params.append('sortByAlphabetical', this.votingParams.sortByAlphabetical);
    }

    if (this.votingParams.sortByDate) {
      params = params.append('sortByDate', this.votingParams.sortByDate);
    }

    if (this.votingParams.sortByVotes) {
      params = params.append('sortByVotes', this.votingParams.sortByVotes);
    }

    params = params.append('pageNumber', this.votingParams.pageNumber.toString());
    params = params.append('pageSize', this.votingParams.pageSize.toString());

    return this.http.get<IPaginationVoting>(this.apiUrl + 'votings', { observe: 'response', params })
      .pipe(
        map(response => {
          this.votings = [...this.votings, ...response.body!.data];
          this.pagination = response.body!;
          return this.pagination;
        })
      );
  }

  getVotingParams() {
    return this.votingParams;
  }

  setVotingParams(params: VotingParams) {
    this.votingParams = params;
  }

  getVotintgById(id: number) {
    // const voting = this.votings.find(p => p.id === id);

    // if (voting) {
    //   return of(voting);
    // }
    return this.http.get<IVoting>(this.baseUrl + id);
  }

  getVotingByUsernameParams() {
    return this.votingByUernameParams;
  }

  setVotingByUsernameParams(params: VotingParams) {
    this.votingByUernameParams = params;
  }

  getVotingsById(useCache: boolean) {
    if (useCache === false) {
      this.ownVotings = [];
    }

    if (this.ownVotings.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.ownVotings.length / this.votingByUernameParams.pageSize);

      if (this.votingByUernameParams.pageNumber <= pagesReceived) {
        this.ownPagination.data =
          this.ownVotings.slice((this.votingByUernameParams.pageNumber - 1) * this.votingByUernameParams.pageSize,
            this.votingByUernameParams.pageNumber * this.votingByUernameParams.pageSize);

        return of(this.ownPagination);
      }
    }

    let params = new HttpParams();

    if (this.votingByUernameParams.userId !== 0) {
      params = params.append('userId', this.votingByUernameParams.userId);
    }

    if (this.votingByUernameParams.search) {
      params = params.append('search', this.votingByUernameParams.search);
    }

    if (this.votingParams.sortByAlphabetical) {
      params = params.append('sortByAlphabetical', this.votingParams.sortByAlphabetical);
    }

    if (this.votingParams.sortByDate) {
      params = params.append('sortByDate', this.votingParams.sortByDate);
    }
    
    if (this.votingParams.sortByVotes) {
      params = params.append('sortByVotes', this.votingParams.sortByVotes);
    }

    params = params.append('pageNumber', this.votingByUernameParams.pageNumber.toString());
    params = params.append('pageSize', this.votingByUernameParams.pageSize.toString());

    return this.http.get<IPaginationVoting>(this.apiUrl + 'votings', { observe: 'response', params })
      .pipe(
        map(response => {
          this.ownVotings = [...this.ownVotings, ...response.body!.data];
          this.pagination = response.body!;
          return this.pagination;
        })
      );
  }

  createVoting(createVoting: ICreateVoting, username: string) {
    return this.http.post(this.baseUrl + username + '/create', createVoting);
  }

  deleteVoting(votingId: number, username: string) {
    return this.http.delete(this.baseUrl + username + '/delete/' + votingId);
  }

  updateVoting(votingId: number, updateVoting: IUpdateVoting, username: string) {
    return this.http.put(this.baseUrl + username + '/update/' + votingId, updateVoting);
  }

  addVariant(votingId: number, nameId: number, username: string) {
    return this.http.post<IVoting>(this.baseUrl + username + '/add-name/' + votingId + '/' + nameId, {});
  }

  deleteVariant(votingId: number, nameId: number, username: string) {
    return this.http.post(this.baseUrl + username + '/delete-name/' + votingId + '/' + nameId, {});
  }

  publishVoting(votingId: number, username: string) {
    return this.http.post(this.baseUrl + username + '/publish/' + votingId, {});
  }

  unpublishVoting(votingId: number, username: string) {
    return this.http.post(this.baseUrl + username + '/unpublish/' + votingId, {});
  }
}
