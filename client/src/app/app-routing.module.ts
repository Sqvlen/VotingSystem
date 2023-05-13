import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './_forms/register/register.component';
import { LoginComponent } from './_forms/login/login.component';
import { AuthGuard } from './_guards/auth.guard';
import { TestErrorComponent } from './_errors/test-error/test-error.component';
import { NotFoundComponent } from './_errors/not-found/not-found.component';
import { ServerErrorComponent } from './_errors/server-error/server-error.component';
import { ListComponent } from './votings/list/list.component';
import { UserVotesComponent } from './votings/user-votes/user-votes.component';
import { UserVotingsComponent } from './votings/user-votings/user-votings.component';
import { EditVotingComponent } from './votings/edit-voting/edit-voting.component';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { CreateVotingComponent } from './_forms/create-voting/create-voting.component';
import { VotingDetailComponent } from './votings/voting-detail/voting-detail.component';
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  {
    path: '', runGuardsAndResolvers: 'always', canActivate: [AuthGuard], children: [
      { path: 'list', component: ListComponent },
      { path: 'list/:id', component: VotingDetailComponent },
      { path: 'votes/my', component: UserVotesComponent },
      { path: 'votings/my', component: UserVotingsComponent },
      { path: 'votings/my/create', component: CreateVotingComponent },
      { path: 'votings/my/edit/:id', component: EditVotingComponent, canDeactivate: [PreventUnsavedChangesGuard] }
    ]
  },
  { path: 'home', component: HomeComponent },
  { path: 'errors', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
