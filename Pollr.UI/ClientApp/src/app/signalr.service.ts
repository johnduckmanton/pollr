import { environment } from '../environments/environment';
import { EventEmitter, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

// Signalr message types
const broadcastMessage = 'Broadcast';
const connectionCountMessage = 'ConnectionCount';
const newConnectionMessage= 'NewConnection';
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

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl)
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
        setTimeout(this.startConnection(), 5000);
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
