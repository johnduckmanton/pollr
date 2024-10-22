/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { MessageService } from '../../core/messages/message.service';
import { SignalRService } from '../../core/signalr.service';
import { PollDataService } from '../../core/poll-data.service';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css'],
})
export class ResultsComponent implements OnInit {
  voteSubscription: Subscription;
  canSendMessage: boolean;
  public results;
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private dataService: PollDataService,
    private messageService: MessageService,
    private signalrService: SignalRService
  ) {
    this.subscribeToEvents();
  }

  ngOnInit() {
    const id: number = Number.parseInt(this.route.snapshot.paramMap.get('id'));
    this.isLoading = true;

    // Get the current results
    this.results = this.dataService.getPollResults$(id).subscribe(results => {
      this.results = results;
      console.log(results);
    });

    this.isLoading = false;

  }

  private subscribeToEvents(): void {
    this.signalrService.connectionEstablished.subscribe(() => {
      this.canSendMessage = true;
    });

    // Subscribe to vote messages and update the result dataset when
    // new messages are received
    this.signalrService.voteReceived.subscribe(message => {
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
