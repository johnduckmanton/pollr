/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

import { ConfigurationService } from '../core/configuration/configuration.service';

// Signalr message types
const voteReceivedMessage = 'VoteReceived';
const broadcastMessage = 'Broadcast';
const connectionCountMessage = 'ConnectionCount';
const newConnectionMessage = 'NewConnection';
const resetPollMessage = 'ResetPoll';
const loadQuestionMessage: string = 'LoadQuestion';

@Injectable()
export class SignalRService {
  voteReceived = new EventEmitter<any>();
  loadQuestion = new EventEmitter<any>();
  newConnection = new EventEmitter<number>();
  broadcast = new EventEmitter<any>();
  connectionEstablished = new EventEmitter<Boolean>();
  resetPoll = new EventEmitter<any>();

  public connectionIsEstablished = false;
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

  private startConnection(): any {
    this._hubConnection
      .start()
      .then(() => {
        this.connectionIsEstablished = true;
        console.log('SignalR Hub connection started');
        this.connectionEstablished.emit(true);
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout(this.startConnection(), 5000);
      });
  }

  private registerOnServerEvents(): void {

    // Restart the connection if it closes
    this._hubConnection.onclose(() => {
      this.connectionEstablished.emit(false);
      console.log('Signalr connection lost, retrying...');
      setTimeout(this.startConnection(), 5000);
    });

    this._hubConnection.on(voteReceivedMessage, (data: any) => {
      console.log('### Vote message received');
      console.log(data);
      this.voteReceived.emit(data);
    });

    this._hubConnection.on(loadQuestionMessage, (data: any) => {
      console.log('### Load question message received');
      console.log(data);
      this.loadQuestion.emit(data);
    });

    this._hubConnection.on(newConnectionMessage, (data: number) => {
      console.log('### New connection message received');
      console.log(data);
      this.newConnection.emit(data);
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

  public vote(pollId: string, question, answer): any {
    this._hubConnection.invoke('Vote', pollId, question, answer);
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
