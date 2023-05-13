import { Component, OnInit } from '@angular/core';
import { IUser } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { take } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  public user: IUser = {} as IUser;
  //public member: Member = {} as Member;

  constructor(public accountService: AccountService, private router: Router) {
  }

  ngOnInit(): void {
    this.loadUser();
  }

  signOut() {
    this.accountService.logout();
    this.router.navigateByUrl('/home');
  }

  loadUser() {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.user = user
        }
      }
    })
  }

  // loadMember() {
  //   if (!this.user) return
  //   this.membersService.getUserById(this.user.id).subscribe({
  //     next: member => {
  //       if (member) {
  //         this.member = member;
  //       }
  //     }
  //   })
  // }
}
