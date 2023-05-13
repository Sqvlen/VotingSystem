import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IVoting } from 'src/app/_models/voting';
import { VotingParams } from 'src/app/_models/votingParams';
import { VotingService } from 'src/app/_services/voting.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
  @ViewChild('search') searchTerm: ElementRef = {} as ElementRef;
  votings: IVoting[] = [];
  totalCount: number = {} as number;
  votingParams: VotingParams = {} as VotingParams;

  sortByAlphabeticalOptions = [
    { name: '', value: '' },
    { name: 'Alphabetical: First to Last', value: 'alphaAsc' },
    { name: 'Alphabetical: Last to First', value: 'alphaDesc' },
  ]

  sortByVotesOptions = [
    { name: '', value: '' },
    { name: 'Number of votes: Less to More', value: 'voteAsc' },
    { name: 'Number of votes: More to Less', value: 'voteDesc' },
  ];
  sortByDateOptions = [
    { name: '', value: '' },
    { name: 'Date: Newly to Early', value: 'dateAsc' },
    { name: 'Date: Early to Newly', value: 'dateDesc' },
  ]

  constructor(private votingsService: VotingService) {
    this.votingParams = this.votingsService.getVotingParams();
  }

  ngOnInit(): void {
    this.loadVotings(false);
  }

  loadVotings(useCache = false) {
    this.votingsService.getVotings(useCache).subscribe({
      next: response => {
        if (response) {
          this.votings = response.data;
          this.totalCount = response.count;
        }
      }
    })
  }

  onSearch() {
    this.votingParams = this.votingsService.getVotingParams();
    this.votingParams.search = this.searchTerm.nativeElement.value;
    this.votingParams.pageNumber = 1;
    this.votingsService.setVotingParams(this.votingParams);
    this.loadVotings();
  }

  onPageChanged(event: any) {
    if (this.votingParams.pageNumber !== event) {
      this.votingParams.pageNumber = event;
      this.votingsService.setVotingParams(this.votingParams);
      this.loadVotings(false);
    }
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.votingParams = new VotingParams();
    this.votingsService.setVotingParams(this.votingParams);
    this.loadVotings();
  }
}
