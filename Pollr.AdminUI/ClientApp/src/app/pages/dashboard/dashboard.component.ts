/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from '../../core/messages/message.service';
import { PollDataService } from '../../core/poll-data.service';
import { Poll, PollStatus } from '../../shared/models/poll.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  isLoading = false;
  polls: Poll[] = [];
  public pollStatus = PollStatus;

  constructor(
    private dataService: PollDataService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.getPolls(PollStatus.Undefined);
  }

  getPolls(status: PollStatus): void {
    if (status === PollStatus.Undefined) {
      this.dataService.getAllPolls$().subscribe(
        data => {
          this.polls = data;
          this.isLoading = false;

          if (this.polls.length === 0) {
            this.messageService.add(`There are currently no polls defined.`);
          }
        },
        error => {
          this.isLoading = false;
          this.messageService.add('Error retrieving polls data');
        }
      );
    } else {
      this.dataService.getPollsByStatus$(status).subscribe(
        data => {
          this.polls = data;
          this.isLoading = false;

          if (this.polls.length === 0) {
            this.messageService.add(`There are currently no ${status.toString()} polls`);
          }
        },
        error => {
          this.isLoading = false;
          this.messageService.add('Error retrieving polls data');
        }
      );
    }
  }

  showInfo(index: number) {
    this.router.navigate(['/poll-details', this.polls[index].id]);
  }

  showVoteStatus(index: number) {
    this.router.navigate(['/vote-status', this.polls[index].id]);
  }

  startPoll(index: number): void {
    this.dataService.openPoll$(this.polls[index].id).subscribe(() => {
      this.polls[index].status = PollStatus.Open;
      this.toastr.success('Your poll has been started. You can now accept votes.');
    });
  }

  stopPoll(index: number): void {
    this.dataService.closePoll$(this.polls[index].id).subscribe(() => {
      this.polls[index].status = PollStatus.Closed;
      this.toastr.warning('Your poll has been stopped. Voting is no longer allowed.');
    });
  }

  nextQuestion(index: number): void {
    this.dataService.nextQuestion$(this.polls[index].id).subscribe(
      updatedPoll => {
        this.polls[index] = updatedPoll;
        console.log(`updated poll: ${updatedPoll}`);
        this.toastr.success(
          `Question ${updatedPoll.currentQuestion} is now available for votes`
        );
      },
      error => {
        this.toastr.error(error);
      }
    );
  }

  showResults(index: number) {
    this.router.navigate(['/results', this.polls[index].id]);
  }
}
