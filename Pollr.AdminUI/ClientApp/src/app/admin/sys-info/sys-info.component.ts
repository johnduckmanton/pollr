import { Component, OnInit } from '@angular/core';

import { ConfigurationService } from '../../core/configuration/configuration.service';
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
  private apiUrl = this.configService.config.apiUrl;
  private appVersion = this.configService.config.appVersion;



  constructor(
    private configService: ConfigurationService,
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
