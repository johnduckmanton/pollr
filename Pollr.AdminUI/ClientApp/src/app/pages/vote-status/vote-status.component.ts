/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

import { ConfigurationService } from '../../core/configuration/configuration.service';
import { SignalRService } from '../../core/signalr.service';
import { PollDataService } from '../../core/poll-data.service';
import { Poll } from '../../shared/models/poll.model';
import { Question } from '../../shared/models/question.model';


@Component({
  selector: 'app-vote-status',
  templateUrl: './vote-status.component.html',
  styleUrls: ['./vote-status.component.css']
})
export class VoteStatusComponent implements OnInit {
  connectedUserCount = 0;

  isLoading = false;
  poll: Poll = null;
  currentQuestion: Question;
  qrcodeElementType = 'url';
  pollVoteUrl = '';

  constructor(
    private configService: ConfigurationService,
    private spinner: NgxSpinnerService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,
    private dataService: PollDataService,
    private signalrService: SignalRService
  ) {
    this.subscribeToEvents();
  }

  ngOnInit() {

    this.isLoading = true;

    const id: number = Number.parseInt(this.route.snapshot.paramMap.get('id'));

    this.dataService.getPoll$(id).subscribe(poll => {
      this.poll = poll;
      this.currentQuestion = this.poll.questions[this.poll.currentQuestion - 1];

      // Calculate the total votes
      this.currentQuestion.totalVotes = this.currentQuestion.answers.map(answer => answer.voteCount).reduce(function sum(prev, next) {
        return prev + next;
      });

      this.pollVoteUrl = this.configService.config.voteUrl;
      this.isLoading = false;
    });
  }

  private subscribeToEvents(): void {

    // Subscribe to vote messages and update the result dataset when
    // new messages are received
    this.signalrService.resultsReceived.subscribe(message => {

      // We will get back a result for the whole poll so we need to
      // dig out the current question from the results and update the vote counts
      this.currentQuestion = message.questions[this.poll.currentQuestion - 1];
    });

    // Subscribe to new connection messages and update the connectedUsers count
    // when new messages are received
    this.signalrService.newConnection.subscribe(count => {
      this.connectedUserCount = count;
    });
  }

}
