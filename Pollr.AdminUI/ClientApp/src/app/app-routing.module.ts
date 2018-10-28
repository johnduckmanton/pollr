import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AboutComponent } from './pages/about/about.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { PollDefinitionListComponent } from './pages/poll-definition-list/poll-definition-list.component';
import { ResultsComponent } from './pages/results/results.component';
import { ViewPollDetailsComponent } from './pages/view-poll-details/view-poll-details.component';
import { VoteStatusComponent } from './pages/vote-status/vote-status.component';
import { PollDefinitionFormComponent } from './pages/poll-definition-form/poll-definition-form.component';

const routes: Routes = [
  { path: 'about', component: AboutComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'poll-definitions', component: PollDefinitionListComponent },
  { path: 'poll-details/:id', component: ViewPollDetailsComponent },
  { path: 'new-poll-definition', component: PollDefinitionFormComponent},
  { path: 'results/:id', component: ResultsComponent },
  { path: 'vote-status/:id', component: VoteStatusComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
