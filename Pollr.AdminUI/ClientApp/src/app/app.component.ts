import { Component, OnInit } from '@angular/core';
import { MessageService } from './core/messages/message.service';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'pollr';

  constructor(private messageService: MessageService) {
    console.log(`Production environment? ${environment.production}`);
  }

  ngOnInit() {}
}
