/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Component, OnInit } from '@angular/core';

import { ConfigurationService } from './core/configuration/configuration.service';
import { MessageService } from './core/messages/message.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'pollr';

  constructor(public configService: ConfigurationService, private messageService: MessageService) {
    console.log(`App version: ${this.configService.config.appVersion}`);
    console.log(`Production environment: ${this.configService.config.production}`);
    console.log(`Api URL: ${this.configService.config.apiUrl}`);
    console.log(`Hub URL: ${this.configService.config.hubUrl}`);
    console.log(`Vote URL: ${this.configService.config.voteUrl}`);
  }

  ngOnInit() { }
}
