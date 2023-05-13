import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { IMember } from '../_models/member';
import { UserParams } from '../_models/userParams';
import { map, of } from 'rxjs';
import { IPaginationUser, PaginationUser } from '../_models/paginationUser';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private baseUrl = environment.apiUrl;

  members: IMember[] = [];
  pagination = new PaginationUser();
  userParams = new UserParams();

  constructor(private http: HttpClient) { }

  getUsers(useCache: boolean) {
    if (useCache === false) {
      this.members = [];
    }

    if (this.members.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.members.length / this.userParams.pageSize);

      if (this.userParams.pageNumber <= pagesReceived) {
        this.pagination.data =
          this.members.slice((this.userParams.pageNumber - 1) * this.userParams.pageSize,
            this.userParams.pageNumber * this.userParams.pageSize);

        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.userParams.userId !== 0) {
      params = params.append('userId', this.userParams.userId.toString());
    }

    if (this.userParams.search) {
      params = params.append('search', this.userParams.search);
    }

    if (this.userParams.username) {
      params = params.append('username', this.userParams.username);
    }

    if (this.userParams.sort) {
      params = params.append('sort', this.userParams.sort);
    }

    params = params.append('pageIndex', this.userParams.pageNumber.toString());
    params = params.append('pageSize', this.userParams.pageSize.toString());

    return this.http.get<IPaginationUser>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(
        map(response => {
          this.members = [...this.members, ...response.body!.data];
          this.pagination = response.body!;
          return this.pagination;
        })
      );
  }

  getMemberParams() {
    return this.userParams;
  }

  setMemberParams(params: UserParams) {
    this.userParams = params;
  }

  getUserById(id: number) {
    return this.http.get<IMember>(this.baseUrl + 'users/' + id);
  }

  getUserByUsername(username: string) {
    return this.http.get<IMember>(this.baseUrl + 'users/' + username);
  }
}
