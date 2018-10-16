import { Component } from '@angular/core';

import { ConfigurationService } from './core/configuration/configuration.service';
import { MessageService } from './message.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'pollr';

  constructor(public configService: ConfigurationService, private messageService: MessageService) {
    console.log(`Production environment: ${this.configService.config.production}`);
    console.log(`Api URL: ${this.configService.config.apiUrl}`);
    console.log(`Hub URL: ${this.configService.config.hubUrl}`);
  }

  ngOnInit() {

  }

}
