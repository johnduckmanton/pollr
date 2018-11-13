/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Answer } from './answer.model';

export class Question {
  id: number;
  questionText: string;
  isDisabled: boolean;
  totalVotes: number;
  answers: Answer[];
}
