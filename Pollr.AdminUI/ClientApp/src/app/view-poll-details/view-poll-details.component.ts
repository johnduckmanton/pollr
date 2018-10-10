import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { NgxSpinnerService } from 'ngx-spinner';

import { Router, ActivatedRoute } from '@angular/router';
import { PollDataService } from '../poll-data.service';
import { MessageService } from '../message.service';

import { Poll } from '../poll.model';

@Component({
  selector: 'app-view-poll-details',
  templateUrl: './view-poll-details.component.html',
  styleUrls: ['./view-poll-details.component.css']
})
export class ViewPollDetailsComponent implements OnInit {

  poll: Poll = null;
  isLoading: boolean = false;
  qrcodeElementType: string = 'url';
  pollVoteUrl: string = '';

  constructor(
    private spinner: NgxSpinnerService,
    private router: Router,
    private route: ActivatedRoute,
    private location: Location,
    private dataService: PollDataService,
    private messageService: MessageService
) { }

  ngOnInit() {
    this.spinner.show();
    this.isLoading = true;
    const id: string = this.route.snapshot.paramMap.get('id');

    this.dataService.getPoll(id).subscribe(
      poll => {
        this.poll = poll;

        // Generate URL to this poll
        const urlTree = this.router.createUrlTree(['/vote', this.poll.handle]).toString();
        const path = this.location.prepareExternalUrl(urlTree.toString());
        this.pollVoteUrl = window.location.origin + path;

        this.spinner.hide();
        this.isLoading = false;
      });



  }

}
