import { Component, Input, OnInit } from '@angular/core';
import { IName } from 'src/app/_models/name';
import { IVote } from 'src/app/_models/vote';
import { IVoting } from 'src/app/_models/voting';
import { NameService } from 'src/app/_services/name.service';
import { VotingService } from 'src/app/_services/voting.service';

@Component({
  selector: 'app-user-votes-card',
  templateUrl: './user-votes-card.component.html',
  styleUrls: ['./user-votes-card.component.css']
})
export class UserVotesCardComponent implements OnInit {
  @Input() vote: IVote = {} as IVote;

  voting: IVoting = {} as IVoting;
  name: IName = {} as IName;

  constructor (private nameService: NameService, private votingService: VotingService) {}

  ngOnInit(): void {
    this.loadName();
    this.loadVoting();
  }

  loadName() {
    this.nameService.getNameById(this.vote.nameId).subscribe({
      next: response => {
        if (response) {
          this.name = response;
        }
      }
    })
  }

  loadVoting() {
    this.votingService.getVotintgById(this.vote.votingId).subscribe({
      next: response => {
        if (response) {
          this.voting = response;
        }
      }
    });
  }
}
