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
  resultsReceived = new EventEmitter<any>();
  newConnection = new EventEmitter<number>();
  broadcast = new EventEmitter<any>();
  connectionEstablished = new EventEmitter<Boolean>();
  resetPoll = new EventEmitter<any>();

  private connectionIsEstablished = false;
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
    this._hubConnection.on(voteReceivedMessage, (data: any) => {
      this.resultsReceived.emit(data);
    });

    this._hubConnection.on(newConnectionMessage, (data: number) => {
      this.newConnection.emit(data);
    });

    this._hubConnection.on(resetPollMessage, (data: number) => {
      this.resetPoll.emit(data);
    });

    this._hubConnection.on(broadcastMessage, (data: any) => {
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
