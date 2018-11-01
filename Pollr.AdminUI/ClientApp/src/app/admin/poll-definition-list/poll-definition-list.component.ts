import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Title } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';

import { MessageService } from '../../core/messages/message.service';
import { PollDataService } from '../../core/poll-data.service';
import { PollDefinition } from '../../shared/models/poll-definition.model';

@Component({
  selector: 'app-poll-definition-list',
  templateUrl: './poll-definition-list.component.html',
  styleUrls: ['./poll-definition-list.component.css']
})
export class PollDefinitionListComponent implements OnInit {
  pageTitle = 'Poll Definitions';
  isLoading = false;
  pollDefinitions: PollDefinition[] = [];

  constructor(
    private dataService: PollDataService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute,
    private spinner: NgxSpinnerService,
    private title: Title,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.title.setTitle(this.pageTitle);
    this.getPollDefinitions();
  }

  getPollDefinitions(): void {
    this.spinner.show();

    this.dataService.getPollDefinitions$().subscribe(
      data => {
        this.pollDefinitions = data;
        this.isLoading = false;

        this.spinner.hide();
        if (this.pollDefinitions.length === 0) {
          this.messageService.add(`There are currently no poll definitions defined.`);
        }
      },
      error => {
        this.spinner.hide();
        this.isLoading = false;
        this.messageService.add('Error retrieving poll definition data');
      }
    );
  }

  createPollDefinition(): void {
    this.router.navigate(['/admin/poll-definitions/create']);


  }
  editPollDefinition(id: string): void {
    this.router.navigate([`/admin/poll-definitions/edit/${id}`]);
  }

  deletePollDefinition(id: string): void {
    this.toastr.success('Delete clicked');

  }
}

