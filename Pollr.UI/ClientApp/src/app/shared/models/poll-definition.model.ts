/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
export class PollDefinition {
  id: string;
  name: string;
  description: string;
  theme: string;
  owner: string;
  isPublished: boolean;
  createDate: Date;
  expiryDate: Date;
  tags: string[];
  questions: [
    {
      questionText: string;
      hasCorrectAnswer: boolean;
      isDisabled: boolean;
      answers: [
        {
          answerText: string;
          imagePath: string;
          isCorrectAnswer: boolean;
          isDisabled: boolean;
        }
      ];
    }
  ];
}
