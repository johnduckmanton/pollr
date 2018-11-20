/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PollDataService } from '../../core/poll-data.service';
import { PollRequest } from 'src/app/shared/models/poll-request.model';

@Component({
  selector: 'app-create-poll',
  templateUrl: './create-poll.component.html',
  styleUrls: ['./create-poll.component.css'],
})
export class CreatePollComponent implements OnInit {
  constructor(
    private activeRoute: ActivatedRoute,
    private dataService: PollDataService,
    private router: Router,
    private toastr: ToastrService
  ) { }

  isEditing: false;
  pollRequest: PollRequest = new PollRequest();
  pollDefinitionId: number;
  pollDefinitionName: string;

  ngOnInit() {
    this.pollDefinitionId = +this.activeRoute.snapshot.queryParamMap.get(
      'pollDefinitionId'
    );

    this.pollDefinitionName = this.activeRoute.snapshot.queryParamMap.get(
      'pollDefinitionName'
    );
  }

  onSubmit() {

    this.pollRequest.pollDefinitionId = this.pollDefinitionId;

    this.dataService.createPoll$(this.pollRequest).subscribe(
      () => {
        this.toastr.success(`Poll has been created.`);
        this.router.navigate(['/dashboard']);
      },
      error => {
        this.toastr.error(error);
      }
    );
  }

  onCancel() {
    this.router.navigateByUrl('/admin/poll-definitions');
  }
}
