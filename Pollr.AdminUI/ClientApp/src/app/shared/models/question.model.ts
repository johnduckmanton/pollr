/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
export class Question {
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
