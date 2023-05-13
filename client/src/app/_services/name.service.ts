import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { IName } from '../_models/name';
import { IPaginationName, PaginationName } from '../_models/paginationName';
import { NameParams } from '../_models/nameParams';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NameService {
  private baseUrl = environment.apiUrl;

  names: IName[] = [];
  pagination = new PaginationName();
  nameParams = new NameParams();

  constructor(private http: HttpClient) { }

  getNames(useCache: boolean) {
    if (useCache === false) {
      this.names = [];
    }

    if (this.names.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.names.length / this.nameParams.pageSize);

      if (this.nameParams.pageNumber <= pagesReceived) {
        this.pagination.data =
          this.names.slice((this.nameParams.pageNumber - 1) * this.nameParams.pageSize,
            this.nameParams.pageNumber * this.nameParams.pageSize);

        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.nameParams.userId !== 0) {
      params = params.append('userId', this.nameParams.userId.toString());
    }

    if (this.nameParams.search) {
      params = params.append('search', this.nameParams.search);
    }

    if (this.nameParams.sort) {
      params = params.append('sort', this.nameParams.sort);
    }

    params = params.append('pageNumber', this.nameParams.pageNumber.toString());
    params = params.append('pageSize', this.nameParams.pageSize.toString());

    return this.http.get<IPaginationName>(this.baseUrl + 'names', { observe: 'response', params })
      .pipe(
        map(response => {
          this.names = [...this.names, ...response.body!.data];
          this.pagination = response.body!;
          return this.pagination;
        })
      );
  }

  getNameParams() {
    return this.nameParams;
  }

  setNameParams(params: NameParams) {
    this.nameParams = params;
  }

  getNameById(nameId: number) {
    // const name = this.names.find(p => p.id === nameId);

    // if (name) {
    //   return of(name);
    // }
    return this.http.get<IName>(this.baseUrl + 'names/' + nameId);
  }

  getNameByUsername(username: string) {
    return this.http.get<IName>(this.baseUrl + 'names/' + username);
  }

  getNameByVotingId(votingId: number) {
    return this.http.get<IName[]>(this.baseUrl + 'votings/' + votingId + '/names');
  }
}
