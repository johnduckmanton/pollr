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
