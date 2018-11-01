import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subscription } from 'rxjs';

import { MessageService } from '../../core/messages/message.service';
import { SignalRService } from '../../core/signalr.service';
import { PollDataService } from '../../core/poll-data.service';

@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.css'],
})
export class ResultComponent implements OnInit {
  voteSubscription: Subscription;
  canSendMessage: boolean;
  public results;

  constructor(
    private spinner: NgxSpinnerService,
    private route: ActivatedRoute,
    private location: Location,
    private dataService: PollDataService,
    private messageService: MessageService,
    private signalrService: SignalRService
  ) {
    this.subscribeToEvents();
  }

  ngOnInit() {
    this.spinner.show();
    const id: string = this.route.snapshot.paramMap.get('id');

    // Get the current results
    this.results = this.dataService.getPollResults$(id).subscribe(results => {
      this.results = results;
      console.log(results);
    });

    this.spinner.hide();
  }

  private subscribeToEvents(): void {
    this.signalrService.connectionEstablished.subscribe(() => {
      this.canSendMessage = true;
    });

    // Subscribe to vote messages and update the result dataset when
    // new messages are received
    this.signalrService.resultsReceived.subscribe(message => {
      console.log(message);
      this.results = message;
    });

    // Subscribe to vote messages and update the result dataset when
    // new messages are received
    this.signalrService.newConnection.subscribe(message => {
      console.log(message);
    });
  }
}
