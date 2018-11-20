/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Question } from './question.model';

export class Poll {
  id: number = 0;
  name: string;
  handle: string;
  description: string;
  status: PollStatus;
  pollDefinitionId: string;
  pollDate: Date;
  currentQuestion: number;
  questions: Question[];
}

export enum PollStatus {
  Undefined = 0,
  Open,
  Closed,
}
