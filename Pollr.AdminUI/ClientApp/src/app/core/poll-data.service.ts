import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { ConfigurationService } from './configuration/configuration.service';
import { MessageService } from './messages/message.service';
import { Poll } from '../shared/models/poll.model';
import { PollDefinition } from '../shared/models/poll-definition.model';


const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};

@Injectable({
  providedIn: 'root',
})
export class PollDataService {
  private apiUrl = this.configService.config.apiUrl;

  constructor(private configService: ConfigurationService,
    private http: HttpClient, private messageService: MessageService) { }

  //
  // Get all poll definitions. GET /api/polls
  //
  public getPollDefinitions(): Observable<PollDefinition[]> {
    const url = `${this.apiUrl}/polldefinitions`;
    return this.http.get<PollDefinition[]>(url);
  }

  //
  // Get all polls. GET /api/polls
  //
  public getAllPolls(): Observable<Poll[]> {
    const url = `${this.apiUrl}/polls`;
    return this.http.get<Poll[]>(url);
  }

  //
  // Get all open polls. GET /api/polls/
  //
  public getPollsByStatus(status: string): Observable<Poll[]> {
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
  // Open a poll
  //
  public openPoll(pollId: string): Observable<any> {
    const url = `${this.apiUrl}/polls/${pollId}/actions/open`;
    return this.http.put(url, {}).pipe(catchError(this.handleError));
  }

  //
  // Close a poll
  //
  public closePoll(pollId: string): Observable<any> {
    const url = `${this.apiUrl}/polls/${pollId}/actions/close`;
    return this.http.put(url, {}).pipe(catchError(this.handleError));
  }

  //
  // Submit a vote
  //
  public vote(pollId: string, question: number, answer: number): Observable<any> {
    const url = `${
      this.apiUrl
    }/polls/${pollId}/actions/vote?question=${question}&answer=${answer}`;

    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };

    return this.http
      .put(url, null, options)
      .pipe(catchError(this.handleError));
  }

  //
  // Advance to next question
  //
  public nextQuestion(pollId: string): Observable<any> {
    const url = `${this.apiUrl}/polls/${pollId}/actions/nextquestion`;
    return this.http.put(url, {}).pipe(catchError(this.handleError));
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
  //private handleError<T>(operation = 'operation', result?: T) {
  //  return (error: any): Observable<T> => {
  //    console.error(error); // log to console instead
  //    this.log(`${operation} failed: ${error.message}`);

  //    // Let the app keep running by returning an empty result.
  //    return of(result as T);
  //  };
  //}

  /** Log a PollService message with the MessageService */
  private log(message: string) {
    this.messageService.add(`PollService: ${message}`);
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.message}`);
    }

    if (error.error) {
      return throwError(`${error.error.errorMessage}`);
    } else {
      return ("An unexpected error has occurred.");
    }
  };
}
