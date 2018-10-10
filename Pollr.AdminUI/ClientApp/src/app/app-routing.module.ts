import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { VoteComponent } from './vote/vote.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ResultsComponent } from './results/results.component';
import { ViewPollDetailsComponent } from './view-poll-details/view-poll-details.component';

const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'poll-details/:id', component: ViewPollDetailsComponent },
  { path: 'vote/:id', component: VoteComponent },
  { path: 'results/:id', component: ResultsComponent },


  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
