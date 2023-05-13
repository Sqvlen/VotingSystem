import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { LoginComponent } from './_forms/login/login.component';
import { RegisterComponent } from './_forms/register/register.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { SharedModule } from './_modules/shared.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';
import { ListComponent } from './votings/list/list.component';
import { UserVotingsComponent } from './votings/user-votings/user-votings.component';
import { UserVotesComponent } from './votings/user-votes/user-votes.component';
import { CardComponent } from './votings/card/card.component';
import { EditCardComponent } from './votings/edit-card/edit-card.component';
import { EditVotingComponent } from './votings/edit-voting/edit-voting.component';
import { CreateVotingComponent } from './_forms/create-voting/create-voting.component';
import { CreateNameComponent } from './_forms/create-name/create-name.component';
import { PagerComponent } from './_forms/pager/pager.component';
import { VotingDetailComponent } from './votings/voting-detail/voting-detail.component';
import { UserVotesCardComponent } from './votings/user-votes/user-votes-card/user-votes-card.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    LoginComponent,
    RegisterComponent,
    TextInputComponent,
    HomeComponent,
    ListComponent,
    UserVotingsComponent,
    UserVotesComponent,
    CardComponent,
    EditCardComponent,
    EditVotingComponent,
    CreateVotingComponent,
    CreateNameComponent,
    PagerComponent,
    VotingDetailComponent,
    UserVotesCardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SharedModule,
    BrowserAnimationsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
