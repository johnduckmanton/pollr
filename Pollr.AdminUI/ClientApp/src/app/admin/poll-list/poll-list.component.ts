/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from '../../core/messages/message.service';
import { PollDataService } from '../../core/poll-data.service';
import { Poll, PollStatus } from '../../shared/models/poll.model';

@Component({
  selector: 'app-poll-list',
  templateUrl: './poll-list.component.html',
  styleUrls: ['./poll-list.component.css'],
})
export class PollListComponent implements OnInit {
  isLoading = false;
  polls: Poll[] = [];
  pollStatus = PollStatus;

  constructor(
    private dataService: PollDataService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.getPolls();
  }

  getPolls(): void {
    this.dataService.getAllPolls$().subscribe(
      data => {
        this.polls = data;
        if (this.polls.length === 0) {
          this.messageService.add(`There are currently no polls defined.`);
        }
      },
      error => {
        this.messageService.add('Error retrieving poll data');
      },
      () => {
        this.isLoading = false;
      }
    );
  }

  editPoll(id: number): void {
    this.router.navigate([`/admin/polls/edit/${id}`]);
  }

  deletePoll(def: Poll): void {

    if (def != null) {
      this.dataService.deletePoll$(def.id).subscribe(
        () => {
          this.polls = this.polls.filter(p => p !== def);
          this.toastr.info('Poll has been deleted', 'Success', {
            closeButton: true
          });
        },
        error => {
          this.toastr.error(`An error occurred: ${error.message}`, 'Sorry', {
            closeButton: true
          });
        }
      );
    }

  }

  publishPoll(id: number): void {
    this.toastr.warning('This feature is not yet implemented', 'Sorry', {
      closeButton: true
    });
  }

  unpublishPoll(id: number): void {
    this.toastr.warning('This feature is not yet implemented', 'Sorry', {
      closeButton: true
    });
  }

  createPoll(): void {
    this.router.navigate(['admin/create-poll']);
  }

  resetPoll(id: number): void {
    this.dataService.resetPoll(id).subscribe(() => {
      this.toastr.warning('Poll vote counts have been reset.');
    });
  }
}
