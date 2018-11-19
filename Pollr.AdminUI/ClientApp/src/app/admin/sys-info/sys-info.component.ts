import { Component, OnInit } from '@angular/core';

import { PollDataService } from '../../core/poll-data.service';
import { MessageService } from '../../core/messages/message.service';

@Component({
  selector: 'app-sys-info',
  templateUrl: './sys-info.component.html',
  styleUrls: ['./sys-info.component.css']
})
export class SysInfoComponent implements OnInit {
  isLoading = false;
  info: any;

  constructor(
    private dataService: PollDataService,
    private messageService: MessageService,
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.dataService.getSysInfo$().subscribe(
      data => {
        this.info = data;
      },
      error => {

        this.messageService.add('Error retrieving poll definition data');
      },
      () => {
        this.isLoading = false;
      }
    );  }

}
