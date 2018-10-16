import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

import { ConfigurationService } from '../core/configuration/configuration.service';
import { MessageService } from '../core/messages/message.service';
import { SignalRService } from '../core/signalr.service';
import { PollDataService } from '../poll-data.service';
import { Poll } from '../poll.model';

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
    this.spinner.show();
    this.isLoading = true;
    const id: string = this.route.snapshot.paramMap.get('id');

    this.dataService.getPoll(id).subscribe(poll => {
      this.poll = poll;

      // Generate URL to this poll
      const urlTree = this.router.createUrlTree(['/vote', this.poll.handle]).toString();
      const path = this.location.prepareExternalUrl(urlTree.toString());
      this.pollVoteUrl = this.configService.config.voteUrl + path;

      this.spinner.hide();
      this.isLoading = false;
    });
  }

  private subscribeToEvents(): void {
    // Subscribe to new connection messages and update the connectedUsers count
    // when new messages are received
    this.signalrService.newConnection.subscribe(count => {
      console.log(count);
      this.connectedUserCount = count;
    });
  }
}
