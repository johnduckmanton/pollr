/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
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

    // Get the current results
    this.results = this.dataService.getPollResults$(id).subscribe(results => {
      this.results = results;
      console.log(results);
    });

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

  }
}
