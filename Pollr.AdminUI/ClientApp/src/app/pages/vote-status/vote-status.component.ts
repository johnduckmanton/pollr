/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { ConfigurationService } from '../../core/configuration/configuration.service';
import { SignalRService } from '../../core/signalr.service';
import { PollDataService } from '../../core/poll-data.service';
import { Poll, PollStatus } from '../../shared/models/poll.model';
import { Question } from '../../shared/models/question.model';
import { MessageService } from '../../core/messages/message.service';


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
  isPollOpen = true;
  connectedToHub: boolean;
  pollStatus = PollStatus;

  constructor(
    private configService: ConfigurationService,
    private dataService: PollDataService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,
    private messageService: MessageService,
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

      // Check the poll is open for voting
      if (this.poll.status != this.pollStatus.Open) {
        this.messageService.add('Poll is not open for voting');
      }

      // Calculate the total votes
      this.currentQuestion.totalVotes = this.currentQuestion.answers.map(answer => answer.voteCount).reduce(function sum(prev, next) {
        return prev + next;
      });

      this.pollVoteUrl = this.configService.config.voteUrl;

      this.isLoading = false;
    });
  }

  private subscribeToEvents(): void {

    // Event to indicate that the signalr hub is ready
    this.signalrService.connectionEstablished.subscribe(() => {
      this.connectedToHub = true;
    });

    // Subscribe to vote messages and update the result dataset when
    // new messages are received
    this.signalrService.loadQuestion.subscribe((message) => {

      if (message) {
        this.currentQuestion = message.questions[message.currentQuestion - 1];
      }
    });

    // Subscribe to vote messages and update the result dataset when
    // new messages are received
    this.signalrService.voteReceived.subscribe((message) => {

      // We will get back a result for the whole poll so we need to
      // dig out the current question from the results and update the vote counts
      this.currentQuestion = message.questions[this.poll.currentQuestion - 1];
    });

    // Subscribe to new connection messages and update the connectedUsers count
    // when new messages are received
    this.signalrService.newConnection.subscribe((count) => {
      this.connectedUserCount = count;
    });

    // Event to indicate that the poll has been reset.
    // Reload the first question in the poll
    this.signalrService.resetPoll.subscribe((message) => {

      if (message) {
        this.currentQuestion = message.questions[message.currentQuestion - 1];
      }
    });
  }

}
