import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { take } from 'rxjs';
import { IUser } from 'src/app/_models/user';
import { IVoting } from 'src/app/_models/voting';
import { VotingParams } from 'src/app/_models/votingParams';
import { AccountService } from 'src/app/_services/account.service';
import { VotingService } from 'src/app/_services/voting.service';

@Component({
  selector: 'app-user-votings',
  templateUrl: './user-votings.component.html',
  styleUrls: ['./user-votings.component.css']
})
export class UserVotingsComponent implements OnInit {
  @ViewChild('search') searchTerm: ElementRef = {} as ElementRef;
  private user: IUser = {} as IUser;
  votings: IVoting[] = [];
  totalCount: number = {} as number;
  votingParams: VotingParams = {} as VotingParams;

  constructor(private votingsService: VotingService, private accountService: AccountService) {
    this.loadUser();
    this.votingParams = this.votingsService.getVotingByUsernameParams();
  }

  ngOnInit(): void {
    this.loadVotings(false);
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

  loadVotings(useCache = false) {
    this.loadUser();

    if (!this.user) return;

    this.votingParams = this.votingsService.getVotingByUsernameParams();

    this.votingParams.userId = this.user.id;
    this.votingsService.setVotingByUsernameParams(this.votingParams);

    this.votingsService.getVotingsById(useCache).subscribe({
      next: response => {
        if (response) {
          this.votings = response.data;
          this.totalCount = response.count;
        }
      }
    });
  }

  onSearch() {
    this.votingParams = this.votingsService.getVotingByUsernameParams();
    this.votingParams.search = this.searchTerm.nativeElement.value;
    this.votingParams.pageNumber = 1;
    this.votingsService.setVotingByUsernameParams(this.votingParams);
    this.loadVotings();
  }

  onPageChanged(event: any) {
    this.votingParams = this.votingsService.getVotingByUsernameParams();
    if (this.votingParams.pageNumber !== event) {
      this.votingParams.pageNumber = event;
      this.votingsService.setVotingByUsernameParams(this.votingParams);
      this.loadVotings(false);
    }
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.votingParams = new VotingParams();
    this.votingsService.setVotingByUsernameParams(this.votingParams);
    this.loadVotings();
  }
}
