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
import { PollDefinition } from '../../shared/models/poll-definition.model';

@Component({
  selector: 'app-poll-definition-list',
  templateUrl: './poll-definition-list.component.html',
  styleUrls: ['./poll-definition-list.component.css'],
})
export class PollDefinitionListComponent implements OnInit {
  isLoading = false;
  pollDefinitions: PollDefinition[] = [];

  constructor(
    private dataService: PollDataService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.isLoading = true;
    this.getPollDefinitions();
  }

  getPollDefinitions(): void {
    this.dataService.getPollDefinitions$().subscribe(
      data => {
        this.pollDefinitions = data;
        if (this.pollDefinitions.length === 0) {
          this.messageService.add(`There are currently no poll definitions defined.`);
        }
      },
      error => {

        this.messageService.add('Error retrieving poll definition data');
      },
      () => {
        this.isLoading = false;
      }
    );
  }

  createPollDefinition(): void {
    this.router.navigate(['/admin/poll-definitions/create']);
  }

  editPollDefinition(id: number): void {
    this.router.navigate([`/admin/poll-definitions/edit/${id}`]);
  }

  deletePollDefinition(def: PollDefinition): void {

    console.log(def);

    if (def != null) {
      this.dataService.deletePollDefinition$(def.id).subscribe(
        () => {
          this.pollDefinitions = this.pollDefinitions.filter(p => p !== def);
          this.toastr.info('Poll definition has been deleted', 'Success', {
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

  publishPollDefinition(id: number): void {
    this.toastr.warning('This feature is not yet implemented', 'Sorry', {
      closeButton: true
    });
  }

  unpublishPollDefinition(id: number): void {
    this.toastr.warning('This feature is not yet implemented', 'Sorry', {
      closeButton: true
    });
  }

  createPoll(def: PollDefinition): void {
    const navigationExtras: NavigationExtras = {
      queryParams: { pollDefinitionId: def.id, pollDefinitionName: def.name },
    };

    this.router.navigate(['admin/create-poll'], navigationExtras);
  }
}
