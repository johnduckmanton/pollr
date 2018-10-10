import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { VoteComponent } from './vote/vote.component';
import { ResultsComponent } from './results/results.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';


const routes: Routes = [
  { path: 'vote/:handle', component: VoteComponent },
  { path: 'results/:id', component: ResultsComponent },
  { path: '**', component: PageNotFoundComponent },
  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
