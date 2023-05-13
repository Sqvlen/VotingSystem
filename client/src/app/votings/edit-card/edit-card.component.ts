import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { IUser } from 'src/app/_models/user';
import { IVoting } from 'src/app/_models/voting';
import { AccountService } from 'src/app/_services/account.service';
import { VotingService } from 'src/app/_services/voting.service';

@Component({
  selector: 'app-edit-card',
  templateUrl: './edit-card.component.html',
  styleUrls: ['./edit-card.component.css']
})
export class EditCardComponent {
  @Input() voting: IVoting = {} as IVoting;
  private user: IUser = {} as IUser;

  constructor(private votingService: VotingService, private router: Router, private accountService: AccountService) {}

  private loadUser() {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.user = user
        }
      }
    })
  }

  publish() {
    if (!this.user) return;

    this.loadUser();

    this.votingService.publishVoting(this.voting.id, this.user.username).subscribe({
      next: () => {
        this.router.navigateByUrl('/list')
      }
    })
  }

  unpublish() {
    if (!this.user) return;

    this.loadUser();

    this.votingService.unpublishVoting(this.voting.id, this.user.username).subscribe({
      next: () => {
        this.router.navigateByUrl('/list')
      }
    })
  }
}
