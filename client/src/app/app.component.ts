import { Component } from '@angular/core';
import { IUser } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Voting System';

  user: IUser = {} as IUser;

  constructor(private accountService: AccountService) {
  }

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = JSON.parse(localStorage.getItem('user')!);
    if (!userString) return;

    const user: IUser = userString;
    this.accountService.setCurrentUser(user);
  }
}
