import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

import { ConfigurationService } from './core/configuration/configuration.service';
import { Promise } from 'q';
import { VoteResult } from './shared/vote-result.model';


// Signalr message types
const broadcastMessage = 'Broadcast';
const connectionCountMessage = 'ConnectionCount';
const newConnectionMessage = 'NewConnection';
const loadQuestionMessage: string = 'LoadQuestion';

@Injectable()
export class SignalRService {

  // Events
  broadcast = new EventEmitter<any>();
  connectionCount = new EventEmitter<any>();
  connectionEstablished = new EventEmitter<Boolean>();
  newConnection = new EventEmitter<any>();
  loadQuestion = new EventEmitter<any>();


  private connectionIsEstablished: boolean = false;
  private _hubConnection: HubConnection;
  private hubUrl = this.configService.config.hubUrl;


  constructor(private configService: ConfigurationService) {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(`${this.hubUrl}`)
      .build();
  }

  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('SignalR Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout(this.startConnection(), 10000);
      });
  }

  private registerOnServerEvents(): void {

    this._hubConnection.on(newConnectionMessage, (data: number) => {
      this.newConnection.emit(data);
    });

    this._hubConnection.on(connectionCountMessage, (data: number) => {
      this.connectionCount.emit(data);
    });

    this._hubConnection.on(loadQuestionMessage, (data: number) => {
      this.loadQuestion.emit(data);
    });

    this._hubConnection.on(broadcastMessage, (data: any) => {
      this.broadcast.emit(data);
    });

  }

  public vote(pollId: number, question, answer) {

    //this._hubConnection
    //  .invoke('Vote', pollId, question, answer)
    //  .then((data) => { result = data.statusCode; console.log(result); })
    //  .catch((error) => { console.log(error); });

    return this._hubConnection
      .invoke('Vote', pollId, question, answer)
      .catch((error) => { console.log(error); });

  }

  public disconnect() {
    if (this._hubConnection) {
      this._hubConnection.stop();
      this._hubConnection = null;
      this.connectionIsEstablished = false;
      this.connectionEstablished.emit(false);
    }
  }
}
