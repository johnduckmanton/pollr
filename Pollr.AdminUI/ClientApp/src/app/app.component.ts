import { Component, OnInit } from '@angular/core';

import { ConfigurationService } from './core/configuration/configuration.service';
import { LoadingService } from './core/loading/loading.service';
import { MessageService } from './core/messages/message.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'pollr';

  constructor(public configService: ConfigurationService, public loaderService: LoadingService, private messageService: MessageService) {
    console.log(`Production environment: ${this.configService.config.production}`);
    console.log(`Api URL: ${this.configService.config.apiUrl}`);
    console.log(`Hub URL: ${this.configService.config.hubUrl}`);
    console.log(`Vote URL: ${this.configService.config.voteUrl}`);
  }

  ngOnInit() { }
}
