/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { throwError as ObservableThrowError, Observable, of } from 'rxjs';
import { ConfigurationService } from './configuration/configuration.service';

import { MessageService } from './messages/message.service';
import { Poll } from '../shared/models/poll.model';
import { PollDefinition } from '../shared/models/poll-definition.model';


@Injectable({
  providedIn: 'root',
})
export class PollDataService {
  private apiUrl = this.configService.config.apiUrl;

  constructor(private configService: ConfigurationService,
    private http: HttpClient, private messageService: MessageService) { }

  //
  // Get all poll definitions. GET /api/polldefinitions
  //
  public getPollDefinitions$(): Observable<PollDefinition[]> {
    const url = `${this.apiUrl}/polldefinitions`;
    return this.http
      .get<PollDefinition[]>(url, this.getOptions())
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Get poll definition. GET /api/polldefinitions/{id}
  //
  public getPollDefinition$(id: string): Observable<PollDefinition[]> {
    const url = `${this.apiUrl}/polldefinitions/${id}`;
    return this.http
      .get<PollDefinition[]>(url, this.getOptions())
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Create a new poll definition. POST /api/polldefinitions
  //
  public savePollDefinition$(pollDefinition: PollDefinition): Observable<PollDefinition> {
    const url = `${this.apiUrl}/polldefinitions`;
    return this.http
      .post<PollDefinition>(url, pollDefinition, this.getOptions())
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // update a new poll definition. PUT /api/polldefinitions/{id}
  //
  public updatePollDefinition$(pollDefinition: PollDefinition): Observable<PollDefinition> {
    const url = `${this.apiUrl}/polldefinitions/${pollDefinition.id}`;
    return this.http
      .put<PollDefinition>(url, pollDefinition, this.getOptions())
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Delete a poll definition. DELETE /api/polldefinitions
  //
  public deletePollDefinition$(id: string): Observable<PollDefinition> {
    const url = `${this.apiUrl}/polldefinitions/${id}`;
    return this.http
      .delete<PollDefinition>(url, this.getOptions())
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Get all polls. GET /api/polls
  //
  public getAllPolls$(): Observable<Poll[]> {
    const url = `${this.apiUrl}/polls`;
    return this.http
      .get<Poll[]>(url)
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Get all polls at the given status. GET /api/polls?status={status}
  //
  public getPollsByStatus$(status: string): Observable<Poll[]> {
    const url = `${this.apiUrl}/polls?status=${status}`;
    return this.http
      .get<Poll[]>(url)
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Get a single poll. GET /api/polls/{id}
  //
  public getPoll$(id: string): Observable<Poll> {
    const url = `${this.apiUrl}/polls/${id}`;
    return this.http
      .get<Poll>(url)
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Get a single poll by its handle. GET /api/polls/?handle={handle}
  //
  public getPollByHandle$(handle: string): Observable<Poll> {
    const url = `${this.apiUrl}/polls?handle=${handle}`;
    return this.http
      .get<Poll>(url)
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Open a poll
  //
  public openPoll$(pollId: string): Observable<any> {
    const url = `${this.apiUrl}/polls/${pollId}/actions/open`;
    return this.http
      .put(url, {})
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Close a poll
  //
  public closePoll$(pollId: string): Observable<any> {
    const url = `${this.apiUrl}/polls/${pollId}/actions/close`;
    return this.http
      .put(url, {})
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Submit a vote
  //
  public vote$(pollId: string, question: number, answer: number): Observable<any> {
    const url = `${this.apiUrl}/polls/${pollId}/actions/vote?question=${question}&answer=${answer}`;

    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };

    return this.http
      .put(url, null, options)
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Advance to next question
  //
  public nextQuestion$(pollId: string): Observable<any> {
    const url = `${this.apiUrl}/polls/${pollId}/actions/nextquestion`;
    return this.http
      .put(url, {})
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  //
  // Get the results for a poll
  //
  public getPollResults$(pollId: string): Observable<any> {
    const url = `${this.apiUrl}/polls/${pollId}/results`;
    return this.http
      .get(url)
      .pipe(
        catchError((error) => this._handleError(error))
      );
  }

  private getOptions() {

    return {
      headers: new HttpHeaders()
        .append('Content-Type', 'application/json')
        // .append('Authorization', `Bearer<${this.auth_token}>`);
    }

  }

  /** Log a PollService message with the MessageService */
  private _log(message: string) {
    this.messageService.add(`PollService: ${message}`);
  }

  // Handle errors
  private _handleError(err: HttpErrorResponse | any): Observable<any> {

    let errorMsg;

    // TODO: Add JWT authentication
    // if (err.message && err.message.indexOf('No JWT present') > -1) {
    //   this.auth.login();
    // }


    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMsg = err.error.message || 'Error: Unable to complete request.';

      console.error(errorMsg);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMsg = err.message || 'Error: Unable to complete request.';

      console.error(
        `Backend returned code ${err.status}, ` +
        `body was: ${err.message}`);
    }

    return ObservableThrowError(errorMsg);

  };
}
