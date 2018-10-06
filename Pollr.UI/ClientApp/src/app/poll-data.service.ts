import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { Poll } from './poll.model';
import { MessageService } from './message.service';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class PollDataService {
  private apiUrl = 'https://localhost:44372/api'; // URL to web api

  constructor(
    private http: HttpClient,
    private messageService: MessageService
  ) { }

  //
  // Get all open polls. GET /api/polls/open
  //
  public getPollsByStatus(status:string): Observable<Poll[]> {
    const url = `${this.apiUrl}/polls/?status=${status}`;
    return this.http.get<Poll[]>(url);
  }


  //
  // Get a single poll. GET /api/polls/{id}
  //
  public getPoll(id: string): Observable<Poll> {
    const url = `${this.apiUrl}/polls/${id}`;
    return this.http.get<Poll>(url);
  }

  //
  // Get a single poll by its handle. GET /api/polls/?handle={handle}
  //
  public getPollByHandle(handle: string): Observable<Poll> {
    const url = `${this.apiUrl}/polls/?handle=${handle}`;
    return this.http.get<Poll>(url);
  }

  //
  // Submit a vote
  //
  public vote(pollId: string, question: number, answer: number): Observable<any>  {

    const url = `${this.apiUrl}/polls/${pollId}/actions/vote?question=${question}&answer=${answer}`;

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.put(url, null, httpOptions )
      .pipe(
      catchError(this.handleError<any>('vote'))
    );

  }

  //
  // Get the results for a poll
  //
  public getPollResults(pollId: string): Observable<any> {
    const url = `${this.apiUrl}/polls/${pollId}/results`;
    return this.http.get(url);
  }

  
  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error); // log to console instead
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log a PollService message with the MessageService */
  private log(message: string) {
    this.messageService.add(`PollService: ${message}`);
  }
}
