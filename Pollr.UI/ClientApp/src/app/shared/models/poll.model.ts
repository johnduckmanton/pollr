/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { PollDefinition } from './poll-definition.model';

export class Poll {
  id: number;
  name: string;
  handle: string;
  description: string;
  status: PollStatus;
  pollDefinitionId: string;
  pollDate: Date;
  currentQuestion: number;
  questions: [
    {
      questionText: string;
      isDisabled: boolean;
      totalVotes: number;
      answers: [
        {
          answerText: string;
          imagePath: string;
          voteCount: number;
          isDisabled: boolean;
        }
      ];
    }
  ];
}

export enum PollStatus {
  Undefined = 0,
  Open,
  Closed,
}
