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
