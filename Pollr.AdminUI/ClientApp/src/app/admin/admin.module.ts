/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TagInputModule } from 'ngx-chips';
import { PollDataService } from '../core/poll-data.service';
import { AdminRoutingModule } from './admin-routing.module';
import { CreatePollComponent } from './create-poll/create-poll.component';
import { PollDefinitionFormComponent } from './poll-definition-form/poll-definition-form.component';
import { PollDefinitionListComponent } from './poll-definition-list/poll-definition-list.component';

// import { QuestionFormComponent } from './poll-definition-form/question-form/question-form.component';
// import { QuestionListComponent } from './poll-definition-form/question-list/question-list.component';

@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    HttpClientModule,
    NgbModule,
    ReactiveFormsModule,
    TagInputModule,
  ],
  declarations: [
    // AnswerListComponent,
    // AnswerFormComponent,
    CreatePollComponent,
    PollDefinitionListComponent,
    PollDefinitionFormComponent,
    // QuestionFormComponent,
    // QuestionListComponent
  ],
  providers: [PollDataService],
})
export class AdminModule {}
