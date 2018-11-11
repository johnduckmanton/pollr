/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { AnswerDefinition } from './answer-definition.model';

export class QuestionDefinition {
  id: number;
  questionText: string;
  hasCorrectAnswer: boolean;
  isDisabled: boolean;
  answers: AnswerDefinition[];
}

