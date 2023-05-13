import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { IName } from 'src/app/_models/name';
import { NameParams } from 'src/app/_models/nameParams';
import { IUser } from 'src/app/_models/user';
import { IVoting } from 'src/app/_models/voting';
import { AccountService } from 'src/app/_services/account.service';
import { NameService } from 'src/app/_services/name.service';
import { VotingService } from 'src/app/_services/voting.service';

@Component({
  selector: 'app-edit-voting',
  templateUrl: './edit-voting.component.html',
  styleUrls: ['./edit-voting.component.css']
})
export class EditVotingComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  };
  voting: IVoting = {} as IVoting;
  totalCount: number = {} as number;
  nameParams: NameParams = {} as NameParams;

  votingNames: IName[] = [];
  names: IName[] = [];

  currentName: string = {} as string;

  private user: IUser = {} as IUser;

  constructor(private router: Router, private route: ActivatedRoute, private votingService: VotingService, private toastr: ToastrService, private nameService: NameService, private accountService: AccountService) {
    this.nameParams = this.nameService.getNameParams();
  }

  ngOnInit(): void {
    this.loadUser();
    this.loadNames(true);
    this.loadVoting();
  }

  loadVoting() {
    var id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    this.votingService.getVotintgById(Number.parseInt(id)).subscribe({
      next: response => {
        if (response) {
          this.voting = response;
          this.loadVotingNames();
        }
      }
    });
  }

  loadVotingNames() {
    this.votingNames = this.voting.names;
  }

  private loadUser() {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.user = user
        }
      }
    })
  }

  loadNames(useCache = false) {
    this.nameService.getNames(useCache).subscribe({
      next: response => {
        if (response) {
          this.names = response.data;
          this.totalCount = response.count;
        }
      }
    })
  }

  updateVoting() {
    this.loadUser();

    this.votingService.updateVoting(this.voting.id, this.editForm?.value, this.user.username).subscribe({
      next: () => {
        this.toastr.success('Voting updated successfully');
        this.editForm?.reset(this.voting);
      },
      error: () => {
        this.toastr.error('Voting didnt update');
        this.editForm?.reset(this.voting);
      }
    })
  }


  deleteVoting() {
    this.votingService.deleteVoting(this.voting.id, this.user.username).subscribe({
      next: () => {
        this.router.navigateByUrl('/votings/my')
      }
    })
  }

  addName() {
    if (!this.currentName) {
      this.toastr.error('Select name for adding');
      return;
    }

    if (this.votingNames.forEach(x => x.id == Number.parseInt(this.currentName[0])) != null) {
      return;
    }

    this.votingService.addVariant(this.voting.id, Number.parseInt(this.currentName[0]), this.user.username).subscribe({
      next: response => {
        if (response) {
          this.voting = response;
          this.loadVoting();
          this.toastr.success('Voting updated successfully');
        }
      }
    })
  }

  deleteName(nameId: number) {
    this.votingService.deleteVariant(this.voting.id, nameId, this.user.username).subscribe({
      next: () => {
        this.loadVoting();
        this.toastr.success('Voting updated successfully');
      }
    })
  }
}
