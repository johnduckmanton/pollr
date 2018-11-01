/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TagInputModule } from 'ngx-chips';

import { AdminRoutingModule } from './admin-routing.module';
//import { AnswerFormComponent } from './poll-definition-form/answer-form/answer-form.component';
//import { AnswerListComponent } from './poll-definition-form/answer-list/answer-list.component';
import { CreatePollDefinitionComponent } from './create-poll-definition/create-poll-definition.component';
import { PollDefinitionFormComponent } from './poll-definition-form/poll-definition-form.component';
import { PollDefinitionListComponent } from './poll-definition-list/poll-definition-list.component';
//import { QuestionFormComponent } from './poll-definition-form/question-form/question-form.component';
//import { QuestionListComponent } from './poll-definition-form/question-list/question-list.component';
import { PollDefinitionRepositoryService } from './poll-definition-repository.service';


@NgModule({
  imports: [
    CommonModule,
    AdminRoutingModule,
    FormsModule,
    HttpClientModule,
    NgbModule,
    ReactiveFormsModule,
    TagInputModule
  ],
  declarations: [
    //AnswerListComponent,
    //AnswerFormComponent,
    CreatePollDefinitionComponent,
    PollDefinitionListComponent,
    PollDefinitionFormComponent,
    //QuestionFormComponent,
    //QuestionListComponent
  ],
  providers: [
    PollDefinitionRepositoryService
  ]
})
export class AdminModule { }
