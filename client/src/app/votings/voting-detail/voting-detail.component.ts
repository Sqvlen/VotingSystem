import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { IName } from 'src/app/_models/name';
import { IUser } from 'src/app/_models/user';
import { IVoting } from 'src/app/_models/voting';
import { AccountService } from 'src/app/_services/account.service';
import { VoteService } from 'src/app/_services/vote.service';
import { VotingService } from 'src/app/_services/voting.service';

@Component({
  selector: 'app-voting-detail',
  templateUrl: './voting-detail.component.html',
  styleUrls: ['./voting-detail.component.css']
})
export class VotingDetailComponent implements OnInit {
  voting: IVoting = {} as IVoting;
  votingNames: IName[] = [];

  private user: IUser = {} as IUser;

  constructor(private toastr: ToastrService, private route: ActivatedRoute, private votingService: VotingService, private voteService: VoteService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.loadVoting();
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

  loadVoting() {
    var id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.votingService.getVotintgById(Number.parseInt(id)).subscribe({
      next: response => {
        if (response) {
          this.voting = response;
          console.log(this.voting)
          this.loadVotingNames();
        }
      }
    });
  }

  loadVotingNames() {
    this.votingNames = this.voting.names;
  }

  vote(nameId: number) {
    this.loadUser();

    if (!this.user) return;

    this.loadVoting();

    if (!this.voting) return;

    this.voteService.vote(this.user.username, this.voting.id, nameId).subscribe({
      next: () => {
        this.toastr.success(this.user.firstname + ' ' + this.user.lastname, 'You vote success');
      }
    })
  }
}
