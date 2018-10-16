import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AboutComponent } from './about/about.component';
import { HomeComponent } from './home/home.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ResultsComponent } from './results/results.component';
import { VoteComponent } from './vote/vote.component';


const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'vote/:handle', component: VoteComponent },
  { path: 'about', component: AboutComponent },
  { path: 'results/:id', component: ResultsComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
