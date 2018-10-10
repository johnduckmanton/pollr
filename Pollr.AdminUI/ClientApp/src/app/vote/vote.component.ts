import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

import { NgxSpinnerService } from 'ngx-spinner';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { Poll } from '../poll.model';
import { PollDefinition } from '../poll-definition.model';
import { PollDataService } from '../poll-data.service';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html',
  styleUrls: ['./vote.component.css']
})
export class VoteComponent implements OnInit {
  poll: Poll = null;
  currentPollDef: PollDefinition;
  currentQuestionIndex = 0;
  currentQuestion;
  currentQuestionDef =
    this.currentPollDef == null
      ? null
      : this.currentPollDef.questions[this.currentQuestionIndex];
  isLoading = false;
  isPollOpen = true;
  isVoteButtonDisabled = true;
  votedMessage: string;
  hasVoted = false;
  selectedAnswer: string;
  selectedAnswerIdx: number = -1;

  // SignalR
  private _hubConnection: HubConnection | undefined;
  public async: any;
  message = '';
  messages: string[] = [];

  constructor(
    private spinner: NgxSpinnerService,
    private route: ActivatedRoute,
    private location: Location,
    private pollDataService: PollDataService,
    private messageService: MessageService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    const id:string = this.route.snapshot.paramMap.get('id');

    // Connect to SignalR hub
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:44372/voteHub')
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this._hubConnection.start().catch(err => console.error(err.toString()));

    this._hubConnection.on('message', (data: any) => {
      const received = `Received: ${data}`;
      this.messageService.add(received);
    });

    // Get the poll Data
    this.getPoll();
    this.spinner.hide();
  }

  getPoll(): void {
    this.spinner.show();

    const id = this.route.snapshot.paramMap.get('id');
    this.pollDataService.getPoll(id)
      .subscribe(poll => {
        this.poll = poll;
        console.log(this.poll);
        this.spinner.hide();
      
        if (this.poll) {
          if (this.poll.status !== 'open') {
            this.isPollOpen = false;
            this.messageService.add('Sorry. This poll is now closed.');
          }
          else {
            // load the first question in the poll
            this.currentQuestionIndex = this.poll.currentQuestion;
            this.currentQuestion = this.poll.questions[
              this.currentQuestionIndex - 1
            ];
          }
        }
      });
  }


  selectAnswer(index) {
    this.selectedAnswerIdx = index;
 }

  vote(votedMessageDlg) {
    this.spinner.show();

    // Vote via SignalR
    const data = `Sent: I voted for ${this.currentQuestion.answers[this.selectedAnswerIdx].answerText}.`;

    if (this._hubConnection) {
      this._hubConnection.invoke('SendVoteUpdate', 'me', data);
    }

    // Vote via REST API
    this.pollDataService.vote(
      this.poll.id,
      this.currentQuestionIndex,
      this.selectedAnswerIdx+1)
      .subscribe(() => {
        this.hasVoted = true;
        this.votedMessage = `You have voted for ${this.currentQuestion.answers[this.selectedAnswerIdx].answerText}.`;
        this.modalService.open(votedMessageDlg, { size: 'sm', centered: true });
      });

    this.spinner.hide();
  }
}
