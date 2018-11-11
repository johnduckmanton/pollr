/*---------------------------------------------------------------------------------------------
 *  Copyright (c) John Duckmanton.
 *  All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { QuestionDefinition } from './question-definition.model';

export class PollDefinition {
  id: number;
  name: string;
  description: string;
  theme: string;
  owner: string;
  isPublished: boolean;
  createDate: Date;
  expiryDate: Date;
  tags: string[];
  questions: QuestionDefinition [];
}
