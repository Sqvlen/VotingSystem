import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { IUser } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { VotingService } from 'src/app/_services/voting.service';

@Component({
  selector: 'app-create-voting',
  templateUrl: './create-voting.component.html',
  styleUrls: ['./create-voting.component.css']
})
export class CreateVotingComponent {
  @Output() cancelLogin = new EventEmitter();
  createForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  private user: IUser = {} as IUser;

  constructor(private formBuilder: FormBuilder, private router: Router, private votingService: VotingService, private accountService: AccountService) { }

  ngOnInit(): void {
    this.initializeForm();
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

  cancel() {
    this.router.navigateByUrl('/votings/my');
  }

  create() {
    this.loadUser();

    this.votingService.createVoting(this.createForm?.value, this.user.username).subscribe({
      next: () => {
        this.router.navigateByUrl('/votings/my')
      }
    });
  }

  initializeForm() {
    this.createForm = this.formBuilder.group({
      title: new FormControl('', Validators.required),
      details: new FormControl('', [Validators.required]),
      description: new FormControl('', [Validators.required])
    });
  }
}
