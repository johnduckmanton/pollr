import { Question } from './question.model';

export class Poll {
  id: string;
  name: string;
  handle: string;
  description: string;
  status: string;
  pollDefinitionId: string;
  pollDate: Date;
  currentQuestion: number;
  questions: Question[];
}
