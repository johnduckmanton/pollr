import { Component, OnInit } from '@angular/core';

import { NgxSpinnerService } from 'ngx-spinner';

import { Poll } from '../poll.model';
import { PollDataService } from '../poll-data.service';
import { MessageService } from '../message.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  isLoading: boolean = false;
  polls: Poll[] = [];

  constructor(
    private spinner: NgxSpinnerService,
    private dataService: PollDataService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.getOpenPolls();
  }


  getOpenPolls(): void {
    this.spinner.show();

    this.dataService.getPollsByStatus('open')
      .subscribe(
        data => {
          this.polls = data;
          this.isLoading = false;

          this.spinner.hide();
          if (this.polls.length == 0) {
            this.messageService.add('There are currently no open polls');
          }
        },
        error => {
          this.spinner.hide();
          this.isLoading = false;
          this.messageService.add('Error retrieving polls data');
        }
      );
  }

  startPoll(): void {
    this.messageService.add('Poll started');
  }

  stopPoll(): void {
    this.messageService.add('Poll stopped');
  }

  nextQuestion(): void {
    this.messageService.add('Next question loaded');
  }
}
