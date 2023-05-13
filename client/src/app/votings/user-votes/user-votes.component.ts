import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs';
import { IUser } from 'src/app/_models/user';
import { IVote } from 'src/app/_models/vote';
import { VoteParams } from 'src/app/_models/voteParams';
import { AccountService } from 'src/app/_services/account.service';
import { VoteService } from 'src/app/_services/vote.service';

@Component({
  selector: 'app-user-votes',
  templateUrl: './user-votes.component.html',
  styleUrls: ['./user-votes.component.css']
})
export class UserVotesComponent implements OnInit {
  votes: IVote[] = [];
  totalCount: number = {} as number;
  voteParams: VoteParams = {} as VoteParams;

  user: IUser = {} as IUser;

  constructor (private voteService: VoteService, private accountService: AccountService) {
    this.voteParams = this.voteService.getVoteParams();
    this.loadUser();
  }

  ngOnInit(): void {
    this.loadVotes();
  }

  loadUser() {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: response => {
        if (response) {
          this.user = response;
        }
      }
    })
  }

  loadVotes() {
    this.loadUser();

    if (!this.user)
    return;

    this.voteService.getVotesById(this.user.id).subscribe({
      next: response => {
        if (response) {
          this.votes = response.data;
          this.totalCount = response.count;
        }
      }
    })
  }

  onPageChanged(event: any) {
    if (this.voteParams.pageNumber !== event) {
      this.voteParams.pageNumber = event;
      this.voteService.setVoteParams(this.voteParams);
      this.loadVotes();
    }
  }
}
