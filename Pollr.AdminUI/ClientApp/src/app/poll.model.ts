import { PollDefinition } from './poll-definition.model';

export class Poll {
  id: string;
  name: string;
  handle: string;
  description: string;
  status: string;
  pollDefinitionId: string;
  pollDate: Date;
  currentQuestion: number;
  questions: [
    {
      questionText: string;
      isDisabled: boolean;
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
