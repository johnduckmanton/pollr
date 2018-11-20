/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { ConfigurationService } from '../../core/configuration/configuration.service';
import { MessageService } from '../../core/messages/message.service';
import { SignalRService } from '../../core/signalr.service';
import { PollDataService } from '../../core/poll-data.service';
import { Poll } from '../../shared/models/poll.model';


@Component({
  selector: 'app-view-poll-details',
  templateUrl: './view-poll-details.component.html',
  styleUrls: ['./view-poll-details.component.css'],
})
export class ViewPollDetailsComponent implements OnInit {
  connectedUserCount = 0;

  poll: Poll = null;
  isLoading = false;
  qrcodeElementType = 'url';
  pollVoteUrl = '';

  constructor(
    private configService: ConfigurationService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,
    private dataService: PollDataService,
    private signalrService: SignalRService
  ) {
    this.subscribeToEvents();
  }

  ngOnInit() {
    const id: number = Number.parseInt(this.route.snapshot.paramMap.get('id'));

    this.dataService.getPoll$(id).subscribe(poll => {
      this.poll = poll;

      // Generate URL to this poll
      const urlTree = this.router.createUrlTree(['/vote', this.poll.handle]).toString();
      const path = this.location.prepareExternalUrl(urlTree.toString());
      this.pollVoteUrl = this.configService.config.voteUrl + path;

    });
  }

  private subscribeToEvents(): void {
    // Subscribe to new connection messages and update the connectedUsers count
    // when new messages are received
    this.signalrService.newConnection.subscribe(count => {
      this.connectedUserCount = count;
    });
  }
}
