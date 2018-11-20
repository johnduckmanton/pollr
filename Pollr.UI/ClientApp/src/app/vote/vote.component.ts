/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Poll, PollStatus } from '../shared/models/poll.model';
import { PollDefinition } from '../shared/models/poll-definition.model';
import { PollDataService } from '../core/poll-data.service';
import { MessageService } from '../core/messages/message.service';
import { SignalRService } from '../core/signalr.service';

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html',
  styleUrls: ['./vote.component.css']
})
export class VoteComponent implements OnInit {
  poll: Poll;
  currentPollDef: PollDefinition;
  currentQuestionIndex = 0;
  currentQuestion;
  currentQuestionDef =
    this.currentPollDef == null
      ? null
      : this.currentPollDef.questions[this.currentQuestionIndex];
  isLoading = false;
  isPollOpen = true;
  canSendMessage: boolean;

  votedMessage: string;
  hasVoted = false;
  selectedAnswer: string;
  selectedAnswerIdx: number = -1;

  constructor(
    private route: ActivatedRoute,
    private pollDataService: PollDataService,
    private messageService: MessageService,
    private signalrService: SignalRService,
    private modalService: NgbModal
  ) {
    this.subscribeToEvents();
  }

  ngOnInit(): void {
    const handle: string = this.route.snapshot.paramMap.get('handle');

    this.isLoading = true;
    this.getPoll(handle);
    this.isLoading = false;
  }

  //
  // Get the poll data from the server
  //
  getPoll(handle: string): void {
    this.pollDataService.getPollByHandle(handle)
      .subscribe(poll => {
        this.poll = poll;
        if (this.poll) {
          if (this.poll.status !== PollStatus.Open) {
            this.isPollOpen = false;
            this.messageService.add('Sorry. This poll is now closed.');
          }
          else {
            // load the current question in the poll
            this.currentQuestionIndex = this.poll.currentQuestion;
            this.loadQuestion();
          }
        } else {
          this.messageService.add('Sorry. Couldn\'t find a poll with that code.');
        }
      });
  }

  //
  // Set the current question
  //
  loadQuestion() {
    // load the current question in the poll
    this.currentQuestion = this.poll.questions[
      this.currentQuestionIndex - 1
    ];
    this.hasVoted = false;
  }

  //
  // Select an answer
  //
  selectAnswer(index: number) {
    this.selectedAnswerIdx = index;
  }

  //
  // Submit the selected answer to the server
  //
  vote(votedMessageDlg: any) {

    console.log(this.canSendMessage);

    if (this.canSendMessage) {
      this.signalrService.vote(this.poll.id, this.currentQuestionIndex, this.selectedAnswerIdx + 1)
        .then((data) => {
          console.log(data);
          let result = data.statusCode;
          console.log(result);

          this.hasVoted = true;
          this.votedMessage = `You have voted for ${this.currentQuestion.answers[this.selectedAnswerIdx].answerText}.`;
          this.modalService.open(votedMessageDlg, { size: 'sm', centered: true });
        });
    }
  }

  //
  // Subscribe to signalr events
  //
  private subscribeToEvents(): void {

    // Event to indicate that the signalr hub is ready
    this.signalrService.connectionEstablished.subscribe(() => {
      this.canSendMessage = true;
    });

    // Event to indicate that the poll has been reset
    this.signalrService.resetPoll.subscribe((data) => {
      this.currentQuestionIndex == data.currentQuestion || 1;
      this.loadQuestion();
    });

    // Event to trigger loading of the next question
    this.signalrService.loadQuestion.subscribe((data) => {
      this.currentQuestionIndex += 1;
      this.loadQuestion();
    });
  }
}


