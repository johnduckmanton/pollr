import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PollDefinitionListComponent } from './poll-definition-list/poll-definition-list.component';
import { PollDefinitionFormComponent } from './poll-definition-form/poll-definition-form.component';


const routes: Routes = [
  { path: 'poll-definitions', component: PollDefinitionListComponent },
  { path: "poll-definitions/:mode/:id", component: PollDefinitionFormComponent },
  { path: "poll-definitions/:mode", component: PollDefinitionFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
