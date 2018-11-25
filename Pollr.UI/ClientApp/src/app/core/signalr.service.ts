/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

import { ConfigurationService } from '../core/configuration/configuration.service';
import { Promise } from 'q';
import { VoteResult } from '../shared/models/vote-result.model';


// Signalr message types
const broadcastMessage = 'Broadcast';
const connectionCountMessage = 'ConnectionCount';
const newConnectionMessage = 'NewConnection';
const resetPollMessage = 'ResetPoll';
const loadQuestionMessage: string = 'LoadQuestion';

@Injectable()
export class SignalRService {

  // Events
  broadcast = new EventEmitter<any>();
  connectionCount = new EventEmitter<any>();
  connectionEstablished = new EventEmitter<Boolean>();
  newConnection = new EventEmitter<any>();
  resetPoll = new EventEmitter<any>();
  loadQuestion = new EventEmitter<any>();


  public connectionIsEstablished: boolean = false;
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
        console.log('### SignalR Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout(this.startConnection(), 10000);
      });
  }

  private registerOnServerEvents(): void {

    this._hubConnection.on(newConnectionMessage, (data: number) => {
      console.log('### New connection message received');
      console.log(data);
      this.newConnection.emit(data);
    });

    this._hubConnection.on(connectionCountMessage, (data: number) => {
      console.log('### Connection count message received');
      console.log(data);
      this.connectionCount.emit(data);
    });

    this._hubConnection.on(loadQuestionMessage, (data: number) => {
      console.log('### Load question message received');
      console.log(data);
      this.loadQuestion.emit(data);
    });

    this._hubConnection.on(resetPollMessage, (data: number) => {
      console.log('### Reset poll message received');
      console.log(data);
      this.resetPoll.emit(data);
    });

    this._hubConnection.on(broadcastMessage, (data: any) => {
      console.log('### Broadcast message received');
      console.log(data);
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
