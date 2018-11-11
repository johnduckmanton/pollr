/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
export class Question {
  id: number;
  questionText: string;
  isDisabled: boolean;
  totalVotes: number;
  answers: [
    {
      id: number;
      answerText: string;
      imagePath: string;
      voteCount: number;
      isDisabled: boolean;
    }
  ];
}
