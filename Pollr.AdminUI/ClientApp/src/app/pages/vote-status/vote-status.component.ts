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
    const id: string = this.route.snapshot.paramMap.get('id');

    this.dataService.getPoll$(id).subscribe(poll => {
      this.poll = poll;
      this.currentQuestion = this.poll.questions[this.poll.currentQuestion - 1];
      console.log(this.currentQuestion);

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
      console.log(count);
      this.connectedUserCount = count;
    });
  }

}
