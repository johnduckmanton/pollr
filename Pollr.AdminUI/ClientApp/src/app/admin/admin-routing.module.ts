/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PollDefinitionFormComponent } from './poll-definition-form/poll-definition-form.component';
import { PollDefinitionListComponent } from './poll-definition-list/poll-definition-list.component';
import { CreatePollComponent } from './create-poll/create-poll.component';

const routes: Routes = [
  { path: 'poll-definitions', component: PollDefinitionListComponent },
  { path: 'poll-definitions/:mode/:id', component: PollDefinitionFormComponent },
  { path: 'poll-definitions/:mode', component: PollDefinitionFormComponent },
  { path: 'create-poll', component: CreatePollComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
